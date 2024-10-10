using Microsoft.AspNetCore.Http;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

/// <summary>
/// Provider for getting and inserting cookies in responses
/// </summary>
public interface ICookieProvider
{
    /// <summary>
    /// Insert fingerprint to cookies for refreshing
    /// </summary>
    /// <param name="response">Instance of response sent to client</param>
    /// <param name="fingerprint">User device fingerprint to insert</param>
    void AddFingerprintCookieToResponse(HttpResponse response, string fingerprint);

    /// <summary>
    /// Insert refresh token to cookies for refreshing
    /// </summary>
    /// <param name="response">Instance of response sent to client</param>
    /// <param name="refreshToken">Refresh token to insert</param>
    void AddRefreshTokenCookieToResponse(HttpResponse response, string refreshToken);
}