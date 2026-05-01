using Wordition.Application.DTO;
using Wordition.Domain.ValueObjects;

namespace Wordition.Application.Interfaces.Services;

public interface IAuthService
{
    public Task<AuthResponse> LoginAsync(string username, string password);
    public Task RegisterAsync(string username, string password, Email? email);
}