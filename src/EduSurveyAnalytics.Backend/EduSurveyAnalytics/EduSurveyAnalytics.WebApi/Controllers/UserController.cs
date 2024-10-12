using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.WebApi.Models.Requests;
using EduSurveyAnalytics.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

[Route("user")]
public class UserController(IUserService userService, IRefreshSessionService refreshSessionService) : BaseController
{
    [Authorize]
    [HttpPost("create")]
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

    [Authorize]
    [HttpPost("set-password")]
    public async Task<Result<None>> SetUserPassword([FromBody] SetUserPasswordRequestModel request)
    {
        var result = await userService.SetUserPasswordAsync(UserId, request.Password);
        return result;
    }

    [Authorize]
    [HttpPost("update")]
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

    [Authorize]
    [HttpPost("delete")]
    public async Task<Result<None>> DeleteUser([FromBody] DeleteUserRequestModel requestModel)
    {
        // Check for permission for creating users
        var hasPermission = (await userService.UserHasPermissionAsync(UserId, UserPermission.EditUsers)).Data;

        if (!hasPermission)
            return Result<None>.Error(ErrorCode.NotPermitted);

        await refreshSessionService.DeleteUserSessionsAsync(requestModel.UserId);
        await userService.DeleteUserAsync(requestModel.UserId);

        return Result<None>.Success();
    }

    [AllowAnonymous]
    [HttpGet("presentation/{userId:guid}")]
    public async Task<Result<GetUserPresentationResponseModel>> GetUserPresentation(Guid userId)
    {
        var presentationResult = await userService.GetUserPresentationAsync(userId);

        return Result<GetUserPresentationResponseModel>.Success(new GetUserPresentationResponseModel
        {
            User = presentationResult.Data
        });
    }
    
}