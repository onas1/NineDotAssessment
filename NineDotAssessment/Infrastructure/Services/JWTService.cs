

using Microsoft.IdentityModel.Tokens;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NineDotAssessment.Infrastructure.Services;

public class JWTService : IJWTService
{
    private readonly IConfiguration _configuration;

    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];
        string issuer = _configuration["JwtSettings:issuer"];
        string audience = _configuration["JwtSettings:audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Define payload (user-specific data)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
            SigningCredentials = signingCredentials,
            Claims = new Dictionary<string, object>
        {
            { "userId", user.Id },
            { "icNumber", user.ICNumber },
            { "name", user.CustomerName },
            { "email", user.Email }
        },
            Issuer = issuer,
            Audience = audience,
            IssuedAt = DateTime.Now,
            
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
