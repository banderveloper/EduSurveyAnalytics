namespace EduSurveyAnalytics.Domain.Entities.Cached;

public class RefreshSession
{
    public Guid UserId { get; set; }
    public string DeviceAddress { get; set; }
    public string DeviceFingerprint { get; set; }
    public string RefreshToken { get; set; }
}