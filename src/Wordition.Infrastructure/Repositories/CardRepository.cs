using Microsoft.EntityFrameworkCore;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Domain.Entities;
using Wordition.Infrastructure.Context;

namespace Wordition.Infrastructure.Repositories;

public class CardRepository : ICardRepository
{
    private readonly WorditionDbContext _db;
    
    public CardRepository(WorditionDbContext  dbContext)
    {
        _db = dbContext;
    }
    
    
    public async Task<List<WorditionCard>> GetCardsAsync(Guid userId)
    {
        var result = await _db.Cards
            .Include(card => card.Translation)
            .Include(card => card.Word)
            .Where(card => card.UserId == userId)
            .ToListAsync();
        return result;
    }

    public async Task<WorditionCard?> GetCardAsync(Guid userId, Guid cardId)
    {
        var result = await _db.Cards
            .Include(card => card.Translation)
            .Include(card => card.Word)
            .Where(card => card.Id == cardId && card.UserId == userId)
            .SingleOrDefaultAsync();
        return result;
    }

    public async Task AddCardAsync(WorditionCard card)
    {
        await _db.Cards.AddAsync(card);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateCardAsync(WorditionCard card)
    {
        var result = await _db.Cards
            .Where(c => c.Id == card.Id && c.UserId == card.UserId)
            .FirstOrDefaultAsync();
        if (result == null) return;
        _db.Entry(result).CurrentValues.SetValues(card);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteCardAsync(Guid userId, Guid cardId)
    {
        await _db.Cards
            .Where(card => card.UserId == userId && card.Id == cardId)
            .ExecuteDeleteAsync();
        await _db.SaveChangesAsync();
    }

    public async Task AddReviewLogAsync(CardReviewLog reviewLog)
    {
        await _db.CardReviewLogs.AddAsync(reviewLog);
        await _db.SaveChangesAsync();
    }

    public async Task<List<CardReviewLog>> GetCardReviewLogsAsync(Guid userId, Guid cardId)
    {
        var result = await _db.CardReviewLogs
            .Include(cardLog => cardLog.WorditionCard)
            .Where(cardLog => cardLog.WorditionCard.UserId == userId && cardLog.WorditionCard.Id == cardId)
            .ToListAsync();
        return result;
    }
}