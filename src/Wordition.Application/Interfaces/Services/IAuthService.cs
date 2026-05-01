using Wordition.Application.DTO;

namespace Wordition.Application.Interfaces;

public interface IAuthService
{
    public Task<AuthResponse> LoginAsync(string username, string password);
    public bool RegisterAsync(string username, string password);
}