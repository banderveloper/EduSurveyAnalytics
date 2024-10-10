namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IRedisKeyProvider
{
    string GetRefreshSessionKey(Guid userId, string fingerprint);
}