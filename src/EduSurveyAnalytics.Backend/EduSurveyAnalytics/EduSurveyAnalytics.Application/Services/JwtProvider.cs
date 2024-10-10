﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduSurveyAnalytics.Application.Configurations;
using EduSurveyAnalytics.Application.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EduSurveyAnalytics.Application.Services;

public class JwtProvider(JwtConfiguration jwtConfiguration) : IJwtProvider
{
    public string GenerateJwtToken(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expirationTime = DateTime.UtcNow.AddMinutes(jwtConfiguration.AccessExpirationMinutes);

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