using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.WebApi.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

[Route("user")]
public class UserController(IUserService userService) : BaseController
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
}