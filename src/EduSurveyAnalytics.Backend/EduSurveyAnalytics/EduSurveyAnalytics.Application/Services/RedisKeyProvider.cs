using EduSurveyAnalytics.Application.Interfaces.Services;

namespace EduSurveyAnalytics.Application.Services;

public class RedisKeyProvider : IRedisKeyProvider
{
    public string GetRefreshSessionKey(Guid userId, string fingerprint)
        => $"{userId}:{fingerprint}";
}