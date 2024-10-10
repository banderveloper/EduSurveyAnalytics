namespace EduSurveyAnalytics.Application.Configurations;

public class CookieConfiguration
{
    public static readonly string ConfigurationKey = "Cookie";
    public string RefreshTokenCookieName { get; set; }
    public string FingerprintCookieName { get; set; }
}