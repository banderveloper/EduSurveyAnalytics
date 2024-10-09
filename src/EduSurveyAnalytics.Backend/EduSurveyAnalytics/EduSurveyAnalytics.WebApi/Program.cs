using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Converters;
using EduSurveyAnalytics.Application.Extensions;
using EduSurveyAnalytics.Persistence;
using EduSurveyAnalytics.WebApi;
using Microsoft.AspNetCore.Mvc;

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
    
    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            // ErrorCode enum int value to snake_case_string in response (ex: not 1, but username_already_exists)
            options.JsonSerializerOptions.Converters.Add(new SnakeCaseStringEnumConverter<ErrorCode>());
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            // Change default 422 behaviour
            options.InvalidModelStateResponseFactory = context =>
            {
                // Get list of validation errors in format {propName: [error1, error2]}
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key.ToLowerFirstLetter(),
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                // Generate server response
                var result = new UnprocessableEntityObjectResult(new Result<Dictionary<string, string[]>>
                {
                    ErrorCode = ErrorCode.InvalidModel,
                    Data = errors
                });
                result.ContentTypes.Add("application/json");

                return result;
            };
        });
}

var app = builder.Build();
{
    app.MapGet("/time", () => DateTime.UtcNow);
    app.MapControllerRoute(name: "default", pattern: "{controller}/{action}");
    
    app.Run();
}

