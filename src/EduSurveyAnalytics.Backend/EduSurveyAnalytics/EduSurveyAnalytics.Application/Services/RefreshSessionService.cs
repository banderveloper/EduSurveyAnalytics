using System.Text.Json;
using EduSurveyAnalytics.Application.Configurations;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities.Cached;
using StackExchange.Redis;

namespace EduSurveyAnalytics.Application.Services;

public class RefreshSessionService(
    IConnectionMultiplexer multiplexer,
    IRedisKeyProvider redisKeyProvider,
    RefreshSessionConfiguration refreshSessionConfiguration)
    : IRefreshSessionService
{
    private readonly IDatabase _redisDatabase = multiplexer.GetDatabase();

    public async Task<Result<None>> CreateOrUpdateSessionAsync(Guid userId, string? deviceAddress,
        string deviceFingerprint,
        string refreshToken)
    {
        // Create redis entity value
        var refreshSession = new RefreshSession
        {
            UserId = userId,
            DeviceAddress = deviceAddress,
            DeviceFingerprint = deviceFingerprint,
            RefreshToken = refreshToken
        };

        // Calculate redis key in format [userId]:[fingerprint]
        var redisKey = redisKeyProvider.GetRefreshSessionKey(userId, deviceFingerprint);

        // Create/update redis entity with ttl
        await _redisDatabase.StringSetAsync(redisKey, JsonSerializer.Serialize(refreshSession),
            TimeSpan.FromMinutes(refreshSessionConfiguration.ExpirationMinutes));

        return Result<None>.Success();
    }
}