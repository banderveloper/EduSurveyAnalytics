using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using EduSurveyAnalytics.Application.Configurations;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EduSurveyAnalytics.Application.Services;

public class JwtProvider : IJwtProvider
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly RefreshSessionConfiguration _refreshSessionConfiguration;
    public TokenValidationParameters ValidationParameters { get; }

    public JwtProvider(JwtConfiguration jwtConfiguration, RefreshSessionConfiguration refreshSessionConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
        _refreshSessionConfiguration = refreshSessionConfiguration;

        Console.WriteLine("JWT CONFIGURATION IN PROVIDER: ");
        Console.WriteLine(JsonSerializer.Serialize(_jwtConfiguration));
        
        ValidationParameters = new TokenValidationParameters
        {
            ValidateTokenReplay = true,
            ValidateIssuer = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtConfiguration.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
        };
    }
    
    public string GenerateToken(Guid userId, JwtType jwtType)
    {
        // generate list of jwt claims, inserting unique key, user id and type of token (access/refresh)
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(JwtRegisteredClaimNames.Typ, jwtType.ToString().ToLower())
        };
        
        // get encryption key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // there are two types of token - access and refresh, they have different ttl
        var expirationTime = jwtType switch
        {
            JwtType.Access => DateTime.UtcNow.AddMinutes(_jwtConfiguration.AccessExpirationMinutes),
            JwtType.Refresh => DateTime.UtcNow.AddMinutes(_refreshSessionConfiguration.ExpirationMinutes),
            _ => throw new ArgumentOutOfRangeException(nameof(jwtType), jwtType, "Unknown jwt type enum value")
        };

        // create token and return it
        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: expirationTime,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    // Validate token
    public bool IsTokenValid(string token, JwtType tokenType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal? principal;
        
        try
        {
            principal = tokenHandler.ValidateToken(token, ValidationParameters, out _);
        }
        catch (Exception)
        {
            return false;
        }

        var tokenTypeClaim = principal.Claims.FirstOrDefault(claim => claim.Type.Equals(JwtRegisteredClaimNames.Typ));
        var tokenTypeValue = tokenTypeClaim?.Value;

        return tokenTypeValue != null && tokenTypeValue.Equals(tokenType.ToString().ToLower());
    }
    
    public Guid GetUserIdFromToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = tokenHandler.ReadJwtToken(jwtToken).Claims;

        var userIdString = claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier)).Value;

        return Guid.Parse(userIdString);
    }
}