namespace EduSurveyAnalytics.Domain.Entities.Cached;

/// <summary>
/// Enduring session of user
/// </summary>
public class RefreshSession
{
    public Guid UserId { get; set; }
    public string? DeviceAddress { get; set; }
    public string DeviceFingerprint { get; set; }
    public string RefreshToken { get; set; }
}