using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities.Cached;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.WebApi.Models.Requests;
using EduSurveyAnalytics.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EduSurveyAnalytics.WebApi.Controllers;

[Route("auth")]
public class AuthController(
    IUserService userService,
    IJwtProvider jwtProvider,
    IRefreshSessionService refreshSessionService,
    ICookieProvider cookieProvider) : BaseController
{
    [HttpPost("sign-in")]
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

    [HttpPost("refresh")]
    public async Task<Result<RefreshResponseModel>> Refresh()
    {
        // ALGORITHM:
        // get refresh token and fingerprint from request cookies, check existing and validity, extract user id from refresh token...
        // ...check session existing, generate tokens, start session, add auth tokens to response cookies 

        // get fingerprint and refreshToken from request cookies
        var fingerprint = cookieProvider.GetFingerprintFromRequestCookie(HttpContext.Request);
        var refreshToken = cookieProvider.GetRefreshTokenFromRequestCookie(HttpContext.Request);

        // check whether they exists
        if (fingerprint is null)
            return Result<RefreshResponseModel>.Error(ErrorCode.InvalidFingerprint);
        if (refreshToken is null)
            return Result<RefreshResponseModel>.Error(ErrorCode.InvalidRefreshToken);

        // if token is not valid - error
        if (!jwtProvider.IsTokenValid(refreshToken, JwtType.Refresh))
            return Result<RefreshResponseModel>.Error(ErrorCode.InvalidRefreshToken);

        // get user id from token
        var userId = jwtProvider.GetUserIdFromToken(refreshToken);

        // check whether session by given user id and fingerprint exists. if not - error
        var sessionExistsResult = await refreshSessionService.SessionExistsAsync(userId, fingerprint);
        var sessionExists = sessionExistsResult.Data;

        if (!sessionExists)
            return Result<RefreshResponseModel>.Error(ErrorCode.SessionNotFound);

        // generate access token for body and refresh token for cookie
        var accessToken = jwtProvider.GenerateToken(userId, JwtType.Access);
        refreshToken = jwtProvider.GenerateToken(userId, JwtType.Refresh);

        // get client ip or null
        var clientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        // update refresh session
        await refreshSessionService.CreateOrUpdateSessionAsync(userId, clientIpAddress, fingerprint, refreshToken);

        // add fingerprint and new refresh to response cookies
        cookieProvider.AddFingerprintCookieToResponse(HttpContext.Response, fingerprint);
        cookieProvider.AddRefreshTokenCookieToResponse(HttpContext.Response, refreshToken);

        // add access token to response body
        return Result<RefreshResponseModel>.Success(new RefreshResponseModel
        {
            AccessToken = accessToken
        });
    }

    [HttpPost("sign-out")]
    public async Task<Result<None>> SignOut()
    {
        // ALGORITHM:
        // get refresh token and fingerprint from request cookies, check their existing and token validity...
        // extract user id from refresh token, delete session, delete cookies

        // get fingerprint and refreshToken from request cookies
        var refreshToken = cookieProvider.GetRefreshTokenFromRequestCookie(HttpContext.Request);
        var fingerprint = cookieProvider.GetFingerprintFromRequestCookie(HttpContext.Request);

        // check whether they exists
        if (refreshToken is null)
            return Result<None>.Error(ErrorCode.InvalidRefreshToken);
        if (fingerprint is null)
            return Result<None>.Error(ErrorCode.InvalidFingerprint);

        // if token is not valid - error
        if (!jwtProvider.IsTokenValid(refreshToken, JwtType.Refresh))
            return Result<None>.Error(ErrorCode.InvalidRefreshToken);

        // get user id from token
        var userId = jwtProvider.GetUserIdFromToken(refreshToken);

        // delete refresh session from user id and fingerprint 
        await refreshSessionService.DeleteSessionAsync(userId, fingerprint);

        // delete fingerprint and refresh token cookies
        cookieProvider.ClearRefreshSessionCookies(HttpContext.Response);

        return Result<None>.Success();
    }

    [Authorize]
    [HttpGet("other-sessions")]
    public async Task<Result<IEnumerable<RefreshSessionPresentationDTO>>> GetOtherRefreshSessions()
    {
        // get user device fingerprint to exclude current session from list
        var fingerprint = cookieProvider.GetFingerprintFromRequestCookie(HttpContext.Request);

        // if fingerprint not found - error
        if (fingerprint is null)
            return Result<IEnumerable<RefreshSessionPresentationDTO>>.Error(ErrorCode.InvalidFingerprint);

        // get all sessions started by user with current id
        var getSessionsResult = await refreshSessionService.GetUserSessionsAsync(UserId);

        // exclude session with current fingerprint, to hide current session for user
        var otherSessions = getSessionsResult.Data
            .Where(dto => !dto.DeviceFingerprint.Equals(fingerprint));

        return Result<IEnumerable<RefreshSessionPresentationDTO>>.Success(otherSessions);
    }
}