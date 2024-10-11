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
        // create redis entity value
        var refreshSession = new RefreshSession
        {
            UserId = userId,
            DeviceAddress = deviceAddress,
            DeviceFingerprint = deviceFingerprint,
            RefreshToken = refreshToken
        };

        // calculate redis key in format [userId]:[fingerprint]
        var redisKey = redisKeyProvider.GetRefreshSessionKey(userId, deviceFingerprint);

        // create/update redis entity with ttl
        await _redisDatabase.StringSetAsync(redisKey, JsonSerializer.Serialize(refreshSession),
            TimeSpan.FromMinutes(refreshSessionConfiguration.ExpirationMinutes));

        return Result<None>.Success();
    }

    public async Task<Result<bool>> SessionExistsAsync(Guid userId, string deviceFingerprint)
    {
        // get redis key [userId]:[fingerprint]
        var redisKey = redisKeyProvider.GetRefreshSessionKey(userId, deviceFingerprint);

        // get value from redis by key
        var redisValue = await _redisDatabase.StringGetAsync(redisKey);

        return Result<bool>.Success(redisValue.HasValue);
    }

    public async Task<Result<None>> DeleteSessionAsync(Guid userId, string deviceFingerprint)
    {
        // get redis key [userId]:[fingerprint]
        var redisKey = redisKeyProvider.GetRefreshSessionKey(userId, deviceFingerprint);

        // drop entity by key
        await _redisDatabase.KeyDeleteAsync(redisKey);

        return Result<None>.Success();
    }

    public async Task<Result<IEnumerable<RefreshSession>>> GetUserSessionsAsync(Guid userId)
    {
        // get key search parrern by user id "[userId]:*"
        var redisKeySearchPattern = redisKeyProvider.GetSearchPatternByUserId(userId);
        // return result
        var refreshSessions = new LinkedList<RefreshSession>();

        // get redis instance
        var server = multiplexer.GetServer(multiplexer.GetEndPoints().First());

        var cursor = 0; 
        do
        {
            // find all redis keys by given pattern
            var keys = server.Keys(cursor, pattern: redisKeySearchPattern, pageSize: 1000);

            foreach (var key in keys)
            {
                // Get the value of each key
                var value = await _redisDatabase.StringGetAsync(key);

                // try convert value into refresh session
                var refreshSession = JsonSerializer.Deserialize<RefreshSession>(value.ToString());

                // if json is not empty - add it to list 
                if (refreshSession is not null)
                    refreshSessions.AddLast(refreshSession);
            }
        } while (cursor != 0);

        return Result<IEnumerable<RefreshSession>>.Success(refreshSessions);
    }
}