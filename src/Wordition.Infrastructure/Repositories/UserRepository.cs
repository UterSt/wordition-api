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

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        var userEntity = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (userEntity == null) return;
        _db.Entry(userEntity).CurrentValues.SetValues(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _db.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _db.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        return await _db.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(r => r.Token == refreshToken);
    }

    public async Task RemoveRefreshTokenAsync(string refreshToken)
    {
        await _db.RefreshTokens.Where(r => r.Token == refreshToken).ExecuteDeleteAsync();
    }

    public async Task RemoveRefreshTokenAsync(Guid id)
    {
        await _db.RefreshTokens.Where(r => r.UserId == id).ExecuteDeleteAsync();
    }
}