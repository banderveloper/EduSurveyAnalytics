using Microsoft.AspNetCore.Http;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface ICookieProvider
{
    void AddFingerprintCookieToResponse(HttpResponse response, string fingerprint);
    void AddRefreshTokenCookieToResponse(HttpResponse response, string refreshToken);
}