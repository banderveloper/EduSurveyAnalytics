namespace EduSurveyAnalytics.Application.Configurations;

public class JwtConfiguration
{
    public static readonly string ConfigurationKey = "Jwt";
    public int AccessExpirationMinutes { get; set; }
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}