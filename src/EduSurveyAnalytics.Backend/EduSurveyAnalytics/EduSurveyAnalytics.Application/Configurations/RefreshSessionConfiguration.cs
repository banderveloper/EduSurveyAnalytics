namespace EduSurveyAnalytics.Application.Configurations;

public class RefreshSessionConfiguration
{
    public static readonly string ConfigurationKey = "RefreshSession";
    public int ExpirationMinutes { get; set; }
}