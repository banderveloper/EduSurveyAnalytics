using EduSurveyAnalytics.Application.Configurations;
using EduSurveyAnalytics.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace EduSurveyAnalytics.Application.Services;

public class CookieProvider(
    CookieConfiguration cookieConfiguration,
    RefreshSessionConfiguration refreshSessionConfiguration) : ICookieProvider
{
    public void AddFingerprintCookieToResponse(HttpResponse response, string fingerprint)
    {
        response.Cookies.Append(cookieConfiguration.FingerprintCookieName, fingerprint,
            new CookieOptions
            {
                Secure = cookieConfiguration.IsSecure,
                HttpOnly = cookieConfiguration.HttpOnly,
                SameSite = cookieConfiguration.IsSameSiteLax ? SameSiteMode.Lax : SameSiteMode.None,
                // temporary until frontend cookie accepting fix
                MaxAge = TimeSpan.MaxValue
            });
    }

    public void AddRefreshTokenCookieToResponse(HttpResponse response, string refreshToken)
    {
        response.Cookies.Append(cookieConfiguration.RefreshTokenCookieName, refreshToken,
            new CookieOptions
            {
                Secure = cookieConfiguration.IsSecure,
                HttpOnly = cookieConfiguration.HttpOnly,
                SameSite = cookieConfiguration.IsSameSiteLax ? SameSiteMode.Lax : SameSiteMode.None,
                // temporary until frontend cookie accepting fix
                MaxAge = TimeSpan.MaxValue
            });
    }

    public string? GetFingerprintFromRequestCookie(HttpRequest request)
    {
        request.Cookies.TryGetValue(cookieConfiguration.FingerprintCookieName, out var fingerprint);
        return fingerprint;
    }

    public string? GetRefreshTokenFromRequestCookie(HttpRequest request)
    {
        request.Cookies.TryGetValue(cookieConfiguration.RefreshTokenCookieName, out var refresh);
        return refresh;
    }

    public void ClearRefreshSessionCookies(HttpResponse response)
    {
        response.Cookies.Delete(cookieConfiguration.FingerprintCookieName);
        response.Cookies.Delete(cookieConfiguration.RefreshTokenCookieName);
    }
}