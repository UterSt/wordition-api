using Wordition.Application.DTO;
using Wordition.Domain.Entities;
using Wordition.Domain.ValueObjects;

namespace Wordition.Application.Interfaces.Services;

public interface IAuthService
{
    public Task<AuthTokenDto> LoginAsync(string username, string password);
    public Task RegisterAsync(string username, string password, Email? email);
    public Task<AuthTokenDto> RefreshAsync(string refreshTokenFromCoockie);
    public Task LogoutAsync(string refreshToken);
}