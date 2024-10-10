using System.Text.Json;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities.Cached;
using StackExchange.Redis;

namespace EduSurveyAnalytics.Application.Services;

public class RefreshSessionService(IConnectionMultiplexer multiplexer, IRedisKeyProvider redisKeyProvider)
    : IRefreshSessionService
{
    private readonly IDatabase _redisDatabase = multiplexer.GetDatabase();

    public async Task<Result<None>> CreateOrUpdateSessionAsync(Guid userId, string deviceAddress, string deviceFingerprint,
        string refreshToken)
    {
        var refreshSession = new RefreshSession
        {
            UserId = userId,
            DeviceAddress = deviceAddress,
            DeviceFingerprint = deviceFingerprint,
            RefreshToken = refreshToken
        };

        var redisKey = redisKeyProvider.GetRefreshSessionKey(userId, deviceFingerprint);

        await _redisDatabase.StringSetAsync(redisKey, JsonSerializer.Serialize(refreshSession));

        return Result<None>.Success();
    }
}