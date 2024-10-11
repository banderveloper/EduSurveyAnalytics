namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IRedisKeyProvider
{
    /// <summary>
    /// Get refresh session redis key using userId and fingerprint
    /// </summary>
    /// <param name="userId">User id, owner of refresh session</param>
    /// <param name="fingerprint">User's device fingerprint</param>
    /// <returns>Built redis key of refresh session that stores user id and fingerprint</returns>
    string GetRefreshSessionKey(Guid userId, string fingerprint);

    /// <summary>
    /// Get refresh session key search pattern by user id "[userId]:*"
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    string GetSearchPatternByUserId(Guid userId);
}