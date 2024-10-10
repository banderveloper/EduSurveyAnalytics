using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Persistence;
using EduSurveyAnalytics.WebApi;

// pgsql datetime fix
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);
{
    // inject custom configuration classes from file DependencyInjection.cs
    builder.AddCustomConfiguration();

    // inject another layers
    builder.Services.AddApplication().AddPersistence();

    // inject controllers with configured api and json behaviour
    builder.AddControllersWithConfiguredBehaviour();
    
    // allowed hosts, implicitly set because of docker
    builder.WebHost.UseUrls("http://*:80", "http://*:5035");
    
    // Get scope for implicit getting instances from ioc container
    var scope = builder.Services.BuildServiceProvider().CreateScope();
    
    // inject redis
    builder.AddRedis(scope);
    
    // ensure created database
    builder.InitializeDatabase(scope);
}

var app = builder.Build();
{
    app.MapGet("/time", () => DateTime.UtcNow);
    app.MapControllerRoute(name: "default", pattern: "{controller}/{action}");

    app.Run();
}