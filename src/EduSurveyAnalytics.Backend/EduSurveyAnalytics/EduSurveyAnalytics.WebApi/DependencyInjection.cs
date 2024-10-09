using EduSurveyAnalytics.Application.Configurations;
using Microsoft.Extensions.Options;

namespace EduSurveyAnalytics.WebApi;

public static class DependencyInjection
{
    // Inject custom configuration classes 
    public static void AddCustomConfiguration(this WebApplicationBuilder builder)
    {
        // Database configuration
        builder.Services.Configure<DatabaseConfiguration>(
            builder.Configuration.GetSection(DatabaseConfiguration.ConfigurationKey));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);

        // Jwt configuration
        builder.Services.Configure<JwtConfiguration>(
            builder.Configuration.GetSection(JwtConfiguration.ConfigurationKey));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<JwtConfiguration>>().Value);
    }
}