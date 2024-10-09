namespace EduSurveyAnalytics.Application.Configurations;

/// <summary>
/// Appsettings configuration interface
/// </summary>
public class DatabaseConfiguration
{
    public static readonly string ConfigurationKey = "Database";
    
    public string ConnectionStringPattern { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}