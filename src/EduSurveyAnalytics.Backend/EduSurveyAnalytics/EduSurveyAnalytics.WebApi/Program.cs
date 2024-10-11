using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Persistence;
using EduSurveyAnalytics.WebApi;

// pgsql datetime fix
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);
{
    /////////// injecting configuration-independent services
    
    // inject custom configuration classes from file DependencyInjection.cs
    builder.AddCustomConfiguration();

    // inject another layers
    builder.Services.AddApplication().AddPersistence();

    // inject controllers with configured api and json behaviour
    builder.AddControllersWithConfiguredBehaviour();
    
    
    /////////// Injecting services dependent from scope-stored configurations
    
    // Get scope for implicit getting instances from ioc container
    var scope = builder.Services.BuildServiceProvider().CreateScope();
    
    // inject redis
    builder.AddRedis(scope);
    
    // Inject jwt authentication
    builder.AddJwtAuthentication(scope);
    
    // ensure created database
    builder.InitializeDatabase(scope);
}

var app = builder.Build();
{
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapGet("/time", () => DateTime.UtcNow);
    app.MapControllerRoute(name: "default", pattern: "{controller}/{action}");

    app.Run();
}