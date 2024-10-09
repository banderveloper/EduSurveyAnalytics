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

    // get dbcontext instance from container and initialize db
    var scope = builder.Services.BuildServiceProvider().CreateScope();
    var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    DatabaseInitializer.Initialize(applicationDbContext);
    
    // allowed hosts, implicitly set because of docker
    builder.WebHost.UseUrls("http://*:80", "http://*:5035");
}

var app = builder.Build();
{
    app.MapGet("/", () => "Hello World!");
    app.MapGet("/time", () => DateTime.UtcNow);
    
    app.Run();
}

