using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EduSurveyAnalytics.Application;

/// <summary>
/// Application layer injection class 
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRefreshSessionService, RefreshSessionService>();
        
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IHashingProvider, ShaHashingProvider>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddSingleton<IRedisKeyProvider, RedisKeyProvider>();
        
        return services;
    }
}