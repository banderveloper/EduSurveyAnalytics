using System.Reflection;
using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.Configurations;
using EduSurveyAnalytics.Application.Converters;
using EduSurveyAnalytics.Application.Extensions;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;

namespace EduSurveyAnalytics.WebApi;

public static class WebApplicationBuilderExtensions
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

        // Redis configuration
        builder.Services.Configure<RedisConfiguration>(
            builder.Configuration.GetSection(RedisConfiguration.ConfigurationKey));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<RedisConfiguration>>().Value);

        // Refresh session configuration
        builder.Services.Configure<RefreshSessionConfiguration>(
            builder.Configuration.GetSection(RefreshSessionConfiguration.ConfigurationKey));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<RefreshSessionConfiguration>>().Value);

        // Cookie session configuration
        builder.Services.Configure<CookieConfiguration>(
            builder.Configuration.GetSection(CookieConfiguration.ConfigurationKey));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<CookieConfiguration>>().Value);
    }

    // Inject controllers with configured json and 422 behaviour
    public static void AddControllersWithConfiguredBehaviour(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                // ErrorCode enum int value to snake_case_string in response (ex: not 1, but username_already_exists)
                options.JsonSerializerOptions.Converters.Add(new SnakeCaseStringEnumConverter<ErrorCode>());
                options.JsonSerializerOptions.Converters.Add(new SnakeCaseStringEnumConverter<UserPermission>());
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

    // Ensure created database
    public static void InitializeDatabase(this WebApplicationBuilder builder, IServiceScope scope)
    {
        var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DatabaseInitializer.Initialize(appDbContext);
        Log.Information("Database successfully checked and initialized");
    }

    // Inject redis
    public static void AddRedis(this WebApplicationBuilder builder, IServiceScope scope)
    {
        var redisConfiguration = scope.ServiceProvider.GetRequiredService<RedisConfiguration>();

        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var configuration = ConfigurationOptions.Parse(redisConfiguration.ConnectionString);
            return ConnectionMultiplexer.Connect(configuration);
        });
    }

    // Inject JWT authentication
    public static void AddJwtAuthentication(this WebApplicationBuilder builder, IServiceScope scope)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters =
                    scope.ServiceProvider.GetRequiredService<IJwtProvider>().ValidationParameters;
            });
    }

    public static void AddLogger(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(config =>
        {
            // xml comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            config.IncludeXmlComments(xmlPath);

            // Input for JWT access token at swagger
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });

            // Inserting written jwt-token to headers
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}