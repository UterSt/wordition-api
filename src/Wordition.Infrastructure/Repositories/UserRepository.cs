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
}