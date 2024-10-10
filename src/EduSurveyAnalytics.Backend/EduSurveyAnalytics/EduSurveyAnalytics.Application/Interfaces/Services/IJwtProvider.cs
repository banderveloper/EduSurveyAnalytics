using EduSurveyAnalytics.Domain.Enums;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IJwtProvider
{
    string GenerateToken(Guid userId, JwtType jwtType);
}