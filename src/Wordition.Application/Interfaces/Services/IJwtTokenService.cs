using Wordition.Domain.Entities;

namespace Wordition.Application.Interfaces;

public interface IJwtTokenService
{
    public string GenerateToken(User user);
}