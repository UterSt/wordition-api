using Microsoft.EntityFrameworkCore;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Domain.Entities;
using Wordition.Infrastructure.Context;

namespace Wordition.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly WorditionDbContext _db;
    public UserRepository(WorditionDbContext  dbContext)
    {
        _db = dbContext;
    }
    public async Task<User?> GetByLoginAsync(string login)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null) return null;
        return user;
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public Task<User?> UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public bool DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Card>> GetAllCardsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Text>> GetAllTextsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _db.RefreshTokens.AddAsync(refreshToken);
        await _db.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        return await _db.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(r => r.Token == refreshToken);
    }

    public async Task RemoveRefreshTokenAsync(string refreshToken)
    {
        await _db.RefreshTokens.Where(r => r.Token == refreshToken).ExecuteDeleteAsync();
        await _db.SaveChangesAsync();
    }

    public async Task RemoveRefreshTokenAsync(Guid id)
    {
        await _db.RefreshTokens.Where(r => r.UserId == id).ExecuteDeleteAsync();
        await _db.SaveChangesAsync();
    }
}