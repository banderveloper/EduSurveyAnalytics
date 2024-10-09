using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Persistence;
using EduSurveyAnalytics.WebApi;

var builder = WebApplication.CreateBuilder(args);
{
    builder.AddCustomConfiguration();
    builder.Services.AddApplication().AddPersistence();

    var scope = builder.Services.BuildServiceProvider().CreateScope();
    var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    DatabaseInitializer.Initialize(applicationDbContext);
    
    builder.WebHost.UseUrls("http://*:80", "http://*:5035");
}

var app = builder.Build();
{
    app.MapGet("/", () => "Hello World!");
    app.MapGet("/time", () => DateTime.UtcNow);
    
    app.Run();
}

