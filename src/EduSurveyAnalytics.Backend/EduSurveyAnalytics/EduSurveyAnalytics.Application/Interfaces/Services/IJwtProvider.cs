using EduSurveyAnalytics.Domain.Enums;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IJwtProvider
{
    /// <summary>
    /// Generate new JWT token
    /// </summary>
    /// <param name="userId">Id of user that will be injected to jwt</param>
    /// <param name="jwtType">Access or refresh</param>
    /// <returns>User's JWT token</returns>
    string GenerateToken(Guid userId, JwtType jwtType);
}