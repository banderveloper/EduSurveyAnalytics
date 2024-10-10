using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.WebApi.Models.Requests;
using EduSurveyAnalytics.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthController(
    IUserService userService,
    IJwtProvider jwtProvider,
    IRefreshSessionService refreshSessionService,
    ICookieProvider cookieProvider) : BaseController
{
    [HttpGet("sign-in")]
    public async Task<Result<SignInResponseModel>> SignIn([FromBody] SignInRequestModel request)
    {
        // ALGORITHM:
        // Get user by credentials, check existing, generate tokens, create/update session, add refresh and fingerprint to response cookies, access token to payload
        var getUserByCredentialsResult =
            await userService.GetUserByCredentialsAsync(request.AccessCode, request.Password);

        if (!getUserByCredentialsResult.Succeed)
            return Result<SignInResponseModel>.Error(getUserByCredentialsResult.ErrorCode);

        var user = getUserByCredentialsResult.Data;

        var accessToken = jwtProvider.GenerateToken(user.Id, JwtType.Access);
        var refreshToken = jwtProvider.GenerateToken(user.Id, JwtType.Refresh);

        var clientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        await refreshSessionService.CreateOrUpdateSessionAsync(user.Id, clientIpAddress, request.Fingerprint,
            refreshToken);

        cookieProvider.AddFingerprintCookieToResponse(HttpContext.Response, request.Fingerprint);
        cookieProvider.AddRefreshTokenCookieToResponse(HttpContext.Response, refreshToken);

        return Result<SignInResponseModel>.Success(new SignInResponseModel
        {
            AccessToken = accessToken,
            PasswordChangeRequired = user.PasswordHash is null
        });
    }
}