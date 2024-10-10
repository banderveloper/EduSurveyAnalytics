namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IRefreshSessionService
{
    /// <summary>
    /// Start or update user's refresh session from given device
    /// </summary>
    /// <param name="userId">User's id to start session</param>
    /// <param name="deviceAddress">Ip address of user's device</param>
    /// <param name="deviceFingerprint">User's device fingerprint for multi-device authentication</param>
    /// <param name="refreshToken">User's refresh token</param>
    /// <returns>Empty result, or error</returns>
    Task<Result<None>> CreateOrUpdateSessionAsync(Guid userId, string? deviceAddress, string deviceFingerprint,
        string refreshToken);
}