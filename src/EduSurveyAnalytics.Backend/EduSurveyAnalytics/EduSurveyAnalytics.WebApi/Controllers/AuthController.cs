using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.WebApi.Models.Requests;
using EduSurveyAnalytics.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthController(IUserService userService, IJwtProvider jwtProvider) : BaseController
{
    [HttpGet("sign-in")]
    public async Task<Result<SignInResponseModel>> SignIn([FromBody] SignInRequestModel request)
    {
        // get user by access code and password
        var getUserResult = await userService.GetUserByCredentialsAsync(request.AccessCode, request.Password);

        // if not succeed - send auth error to client
        if (!getUserResult.Succeed)
            return Result<SignInResponseModel>.Error(getUserResult.ErrorCode);

        // get existing user from result
        var user = getUserResult.Data;

        return Result<SignInResponseModel>.Success(new SignInResponseModel
        {
            AccessToken = jwtProvider.GenerateJwtToken(user.Id),
            PasswordChangeRequired = user.PasswordHash is null
        });
    }
}