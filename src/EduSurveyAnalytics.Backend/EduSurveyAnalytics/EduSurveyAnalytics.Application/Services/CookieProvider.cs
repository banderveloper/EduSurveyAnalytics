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
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(DateTime.UtcNow.AddMinutes(refreshSessionConfiguration.ExpirationMinutes))
            });
    }

    public void AddRefreshTokenCookieToResponse(HttpResponse response, string refreshToken)
    {
        response.Cookies.Append(cookieConfiguration.RefreshTokenCookieName, refreshToken,
            new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(DateTime.UtcNow.AddMinutes(refreshSessionConfiguration.ExpirationMinutes))
            });
    }
}