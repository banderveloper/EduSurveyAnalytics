using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.WebApi.Models.Requests;
using EduSurveyAnalytics.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

/// <summary>
/// User controller
/// </summary>
[Route("user")]
[Produces("application/json")]
public class UserController(IUserService userService, IRefreshSessionService refreshSessionService) : BaseController
{
    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="request">Request with data of new user</param>
    /// <returns>Empty result</returns>
    /// <response code="200">Success / not_permitted / access_code_already_exists</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Request is not valid</response>
    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<Dictionary<string, string[]>>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<Result<None>> CreateUser([FromBody] CreateUserRequestModel request)
    {
        // Check for permission for creating users
        var hasPermission = (await userService.UserHasPermissionAsync(UserId, UserPermission.EditUsers)).Data;

        if (!hasPermission)
            return Result<None>.Error(ErrorCode.NotPermitted);

        var createUserResult = await userService.CreateUserAsync(request.AccessCode, request.LastName,
            request.FirstName, request.MiddleName, request.BirthDate, request.Post);

        return createUserResult.Succeed 
            ? Result<None>.Success() 
            : Result<None>.Error(createUserResult.ErrorCode);
    }

    /// <summary>
    /// Change your password
    /// </summary>
    /// <param name="request">Request with new password</param>
    /// <returns>Empty result</returns>
    /// <response code="200">Success / user_not_found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Request is not valid</response>
    [Authorize]
    [HttpPost("set-password")]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<Dictionary<string, string[]>>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<Result<None>> SetUserPassword([FromBody] SetUserPasswordRequestModel request)
    {
        var result = await userService.SetUserPasswordAsync(UserId, request.Password);
        return result;
    }

    /// <summary>
    /// Update user data
    /// </summary>
    /// <param name="request">Request with new user data</param>
    /// <returns>Empty result</returns>
    /// <response code="200">Success / not_permitted / user_not_found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Request is not valid</response>
    [Authorize]
    [HttpPost("update")]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<Dictionary<string, string[]>>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<Result<None>> UpdateUser([FromBody] UpdateUserRequestModel request)
    {
        // whether user try to update himself (user id from token equals user id from request)
        var isSelfUpdate = UserId.Equals(request.UserId);

        // if not self-update - check user editing permission
        if (!isSelfUpdate)
        {
            var hasPermission = (await userService.UserHasPermissionAsync(UserId, UserPermission.EditUsers)).Data;
            if (!hasPermission)
                return Result<None>.Error(ErrorCode.NotPermitted);
        }

        await userService.UpdateUserAsync(request.UserId, request.AccessCode, request.LastName, request.FirstName,
            request.MiddleName, request.BirthDate, request.Post, request.Permissions);

        return Result<None>.Success();
    }

    /// <summary>
    /// Delete user by user id
    /// </summary>
    /// <param name="request">Request with user id to delete</param>
    /// <returns>Empty result</returns>
    /// <response code="200">Success / not_permitted</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Request is not valid</response>
    [Authorize]
    [HttpPost("delete")]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<Dictionary<string, string[]>>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<Result<None>> DeleteUser([FromBody] DeleteUserRequestModel request)
    {
        // Check for permission for creating users
        var hasPermission = (await userService.UserHasPermissionAsync(UserId, UserPermission.EditUsers)).Data;

        if (!hasPermission)
            return Result<None>.Error(ErrorCode.NotPermitted);

        await refreshSessionService.DeleteUserSessionsAsync(request.UserId);
        await userService.DeleteUserAsync(request.UserId);

        return Result<None>.Success();
    }

    /// <summary>
    /// Delete user by user id
    /// </summary>
    /// <param name="userId">User id to get presentation data</param>
    /// <returns>User presentation data with given id</returns>
    /// <response code="200">Success</response>
    [AllowAnonymous]
    [HttpGet("presentation/{userId:guid}")]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status200OK)]
    public async Task<Result<GetUserPresentationResponseModel>> GetUserPresentation(Guid userId)
    {
        var presentationResult = await userService.GetUserPresentationAsync(userId);

        return Result<GetUserPresentationResponseModel>.Success(new GetUserPresentationResponseModel
        {
            User = presentationResult.Data
        });
    }

    /// <summary>
    /// Get user full data
    /// </summary>
    /// <param name="userId">User id to get full data</param>
    /// <returns>User full data by id</returns>
    /// <response code="200">Success / not_permitted</response>
    /// <response code="401">Unauthorized</response>
    [Authorize]
    [HttpGet("full/{userId:guid}")]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status401Unauthorized)]
    public async Task<Result<GetUserFullDataResponseModel>> GetUserFullData(Guid userId)
    {
        // whether user get info about himself
        var isSelfGet = UserId.Equals(userId);

        // if not self-get - check user permission to get full data
        if (!isSelfGet)
        {
            var hasPermission = (await userService.UserHasPermissionAsync(UserId, UserPermission.GetUsersFullData))
                .Data;
            if (!hasPermission)
                return Result<GetUserFullDataResponseModel>.Error(ErrorCode.NotPermitted);
        }

        var fullDataResult = await userService.GetUserFullDataAsync(userId);

        return Result<GetUserFullDataResponseModel>.Success(new GetUserFullDataResponseModel
        {
            User = fullDataResult.Data
        });
    }
}