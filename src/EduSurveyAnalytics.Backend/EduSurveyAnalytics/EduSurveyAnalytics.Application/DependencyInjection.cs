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
        // Inject services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRefreshSessionService, RefreshSessionService>();
        services.AddScoped<IFormService, FormService>();
        
        // Inject providers
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IHashingProvider, ShaHashingProvider>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddSingleton<IRedisKeyProvider, RedisKeyProvider>();
        services.AddSingleton<ICookieProvider, CookieProvider>();
        
        return services;
    }
}