using EduSurveyAnalytics.Application.Configurations;
using EduSurveyAnalytics.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EduSurveyAnalytics.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var scope = services.BuildServiceProvider().CreateScope();

        // Get database configurations from configurations
        var databaseConfiguration = scope.ServiceProvider.GetRequiredService<DatabaseConfiguration>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // If it is no appsettings for given environment - don't run server
            if (string.IsNullOrEmpty(databaseConfiguration.ConnectionStringPattern))
                throw new Exception("Configuration for environment not found!");
            
            // ConnectionStringPattern looks like ="username={0}, password={1}"
            var filledConnectionString = string.Format(databaseConfiguration.ConnectionStringPattern,
                databaseConfiguration.Username, databaseConfiguration.Password);
            
            options.UseNpgsql(filledConnectionString);
            
            // No ef caching, increases EF perfomance
            // in Update ef quieries required to use .AsTracking() for working
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        
        // Bind interface and earlier injected app db context
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}