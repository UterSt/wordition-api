using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Wordition.Application.Interfaces.Services;
using Wordition.Domain.Entities;

namespace Wordition.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration cofiguration)
    {
        _configuration = cofiguration;
    }
    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Login)
        };
        if (user.Email != null)
        {
            claims.Add(new Claim(ClaimTypes.Email, user.Email.Value));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (string tokenForCookie, string token) GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        RandomNumberGenerator.Fill(randomNumber);
        var tokenForCookie = Convert.ToBase64String(randomNumber)
            .Replace("/", "_")
            .Replace("+", "-")
            .TrimEnd('=');
        var token = GetHashToken(tokenForCookie);
        return (tokenForCookie, token);
    }

    public string GetHashToken(string token)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}