using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.WebApi.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

[Route("user")]
public class UserController(IUserService userService) : BaseController
{
    [HttpPost("create")]
    public async Task<Result<None>> CreateUser([FromBody] CreateUserRequestModel request)
    {
        var createUserResult = await userService.CreateUserAsync(request.AccessCode, request.LastName,
            request.FirstName, request.MiddleName, request.BirthDate, request.Post);

        return createUserResult.Succeed 
            ? Result<None>.Success() 
            : Result<None>.Error(createUserResult.ErrorCode);
    }
}