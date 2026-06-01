using Microsoft.EntityFrameworkCore;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Domain.Entities;
using Wordition.Infrastructure.Context;

namespace Wordition.Infrastructure.Repositories;

public class TextRepository :  ITextRepository
{
    private readonly WorditionDbContext _db;
    
    public TextRepository(WorditionDbContext context)
    {
       _db = context; 
    }
    
    public async Task<List<Text>> GetTextsAsync(Guid userId)
    {
        return await _db.Texts.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<Text?> GetTextAsync(Guid userId, Guid textId)
    {
        return await _db.Texts
            .Where(t => t.UserId == userId)
            .FirstOrDefaultAsync(t => t.Id == textId);;
    }

    public async Task AddTextAsync(Text text)
    {
        await _db.Texts.AddAsync(text);
    }

    public async Task UpdateTextAsync(Text text)
    {
        var textEntity = await _db.Texts.Where(t => t.UserId == text.UserId).FirstOrDefaultAsync(t => t.Id == text.Id);
        if (textEntity == null) return;
        _db.Entry(textEntity).CurrentValues.SetValues(text);
    }

    public async Task DeleteTextAsync(Guid userId, Guid textId)
    {
        await _db.Texts.Where(t => t.Id == textId && t.UserId == userId).ExecuteDeleteAsync();
    }
}