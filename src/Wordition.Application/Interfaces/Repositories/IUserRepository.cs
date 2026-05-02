using Wordition.Domain.Entities;

namespace Wordition.Application.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User?> GetByLoginAsync(string login);
    public Task<User?> GetByIdAsync(Guid id);
    public Task AddUserAsync(User user);
    public Task<User?> UpdateAsync(User user);
    public bool DeleteAsync(Guid id);
    public Task<List<Card>> GetAllCardsAsync();
    public Task<List<Text>> GetAllTextsAsync();
    public Task AddRefreshTokenAsync(RefreshToken refreshToken);
    public Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
    public Task RemoveRefreshTokenAsync(string refreshToken);
    public Task RemoveRefreshTokenAsync(Guid id);
}