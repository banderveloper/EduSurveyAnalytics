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

    /// <summary>
    /// Get device fingerprint from request cookie if it exists
    /// </summary>
    /// <param name="request">Instance of request to get cookies</param>
    /// <returns>Fingerprint from cookie, or null</returns>
    string? GetFingerprintFromRequestCookie(HttpRequest request);

    /// <summary>
    /// Get refresh token from request cookie if it exists
    /// </summary>
    /// <param name="request">Instance of request to get cookies</param>
    /// <returns>Refresh token from cookie, or null</returns>
    string? GetRefreshTokenFromRequestCookie(HttpRequest request);
}