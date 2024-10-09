using EduSurveyAnalytics.Application.Configurations;
using Microsoft.Extensions.Options;

namespace EduSurveyAnalytics.WebApi;

public static class DependencyInjection
{
    public static void AddCustomConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection(DatabaseConfiguration.ConfigurationKey));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
    }
}