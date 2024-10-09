namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IJwtProvider
{
    string GenerateJwtToken(Guid userId);
}