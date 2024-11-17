namespace EduSurveyAnalytics.Application.Configurations;

public class CookieConfiguration
{
    public static readonly string ConfigurationKey = "Cookie";
    public string RefreshTokenCookieName { get; set; }
    public string FingerprintCookieName { get; set; }
    public bool IsSecure { get; set; }
    public bool HttpOnly { get; set; }
    public bool IsSameSiteLax { get; set; }
}