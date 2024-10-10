namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IRefreshSessionService
{
    Task<Result<None>> CreateOrUpdateSessionAsync(Guid userId, string deviceAddress, string deviceFingerprint,
        string refreshToken);
}