using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduSurveyAnalytics.Application.Configurations;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EduSurveyAnalytics.Application.Services;

public class JwtProvider(JwtConfiguration jwtConfiguration, RefreshSessionConfiguration refreshSessionConfiguration) : IJwtProvider
{
    public string GenerateToken(Guid userId, JwtType jwtType)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(JwtRegisteredClaimNames.Typ, jwtType.ToString().ToLower())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirationTime = jwtType switch
        {
            JwtType.Access => DateTime.UtcNow.AddMinutes(jwtConfiguration.AccessExpirationMinutes),
            JwtType.Refresh => DateTime.UtcNow.AddMinutes(refreshSessionConfiguration.ExpirationMinutes),
            _ => throw new ArgumentOutOfRangeException(nameof(jwtType), jwtType, "Unknown jwt type enum value")
        };

        // create token and return it
        var token = new JwtSecurityToken(
            issuer: jwtConfiguration.Issuer,
            audience: jwtConfiguration.Audience,
            claims: claims,
            expires: expirationTime,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}