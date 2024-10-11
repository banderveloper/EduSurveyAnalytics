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

    /// <summary>
    /// Whether session with given user id and fingerprint exists
    /// </summary>
    /// <param name="userId">User id to check session</param>
    /// <param name="deviceFingerprint">User device to check session</param>
    /// <returns>Whether session with given user id and fingerprint exists</returns>
    Task<Result<bool>> SessionExistsAsync(Guid userId, string deviceFingerprint);

    /// <summary>
    /// Delete session entity from redis
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="deviceFingerprint">Fingerprint</param>
    /// <returns>Empty result</returns>
    Task<Result<None>> DeleteSessionAsync(Guid userId, string deviceFingerprint);
}