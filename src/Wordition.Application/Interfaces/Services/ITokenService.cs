using Wordition.Domain.Entities;

namespace Wordition.Application.Interfaces.Services;

public interface ITokenService
{
    public string GenerateJwtToken(User user);
    public (string tokenForCookie, string token) GenerateRefreshToken();
    public string GetHashToken(string token);
}