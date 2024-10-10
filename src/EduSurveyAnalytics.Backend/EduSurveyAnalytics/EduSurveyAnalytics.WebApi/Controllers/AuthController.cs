using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.WebApi.Models.Requests;
using EduSurveyAnalytics.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthController(IUserService userService, IJwtProvider jwtProvider, IRefreshSessionService refreshSessionService) : BaseController
{
    [HttpGet("sign-in")]
    public async Task<Result<SignInResponseModel>> SignIn([FromBody] SignInRequestModel request)
    {
        // ALGORITHM:
        // Get user by credentials, check existing, generate tokens, create/update session, add refresh and fingerprint to response cookies, access token to payload
        await refreshSessionService.CreateOrUpdateSessionAsync(Guid.NewGuid(), "dom", "idk", "REFREESH");

        return Result<SignInResponseModel>.Success(null);
    }
}