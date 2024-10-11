using EduSurveyAnalytics.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IJwtProvider
{
    TokenValidationParameters ValidationParameters { get; }
    
    /// <summary>
    /// Generate new JWT token
    /// </summary>
    /// <param name="userId">Id of user that will be injected to jwt</param>
    /// <param name="jwtType">Access or refresh</param>
    /// <returns>User's JWT token</returns>
    string GenerateToken(Guid userId, JwtType jwtType);

    /// <summary>
    /// Check whether token is valid
    /// </summary>
    /// <param name="token">JWT token to validate</param>
    /// <param name="tokenType">Token type (access or refresh)</param>
    /// <returns>Whether token is valid</returns>
    bool IsTokenValid(string token, JwtType tokenType);

    /// <summary>
    /// Get user id from jwt token
    /// </summary>
    /// <param name="jwtToken">JWT token to extract user id</param>
    /// <returns>User id from JWT token</returns>
    Guid GetUserIdFromToken(string jwtToken);
}