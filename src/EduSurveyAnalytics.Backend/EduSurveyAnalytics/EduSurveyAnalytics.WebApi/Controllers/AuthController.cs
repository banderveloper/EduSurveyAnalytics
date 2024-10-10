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
        
        // get user by access code and password
        var getUserByCredentialsResult =
            await userService.GetUserByCredentialsAsync(request.AccessCode, request.Password);

        // if user not found - error
        if (!getUserByCredentialsResult.Succeed)
            return Result<SignInResponseModel>.Error(getUserByCredentialsResult.ErrorCode);

        // get existing user from result
        var user = getUserByCredentialsResult.Data;

        // generate jwt access and refresh token
        var accessToken = jwtProvider.GenerateToken(user.Id, JwtType.Access);
        var refreshToken = jwtProvider.GenerateToken(user.Id, JwtType.Refresh);

        // get client ip or null
        var clientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        // start/update cached refresh session
        await refreshSessionService.CreateOrUpdateSessionAsync(user.Id, clientIpAddress, request.Fingerprint,
            refreshToken);

        // add refresh token and fingerprint to cookies
        cookieProvider.AddFingerprintCookieToResponse(HttpContext.Response, request.Fingerprint);
        cookieProvider.AddRefreshTokenCookieToResponse(HttpContext.Response, refreshToken);

        // response with access token and requirement of changing a password if it is not set
        return Result<SignInResponseModel>.Success(new SignInResponseModel
        {
            AccessToken = accessToken,
            PasswordChangeRequired = user.PasswordHash is null
        });
    }
}