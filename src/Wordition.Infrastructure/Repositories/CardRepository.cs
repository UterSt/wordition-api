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

    public async Task<List<WorditionCard>> GetDueCardsAsync(Guid userId)
    {
        var learnAheadMinutes = 20;
        var result = await _db.Cards
            .Include(card => card.Translation)
            .Include(card => card.Word)
            .Where(card => card.UserId == userId)
            .Where(card => card.Due <  DateTime.UtcNow.AddMinutes(learnAheadMinutes))
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

    public async Task UpdateCardContentAsync(WorditionCard card)
    {
        var cardEntity = await _db.Cards
            .Include(c => c.Translation)
            .Include(c => c.Word)
            .Where(c => c.Id == card.Id && c.UserId == card.UserId)
            .SingleOrDefaultAsync();
        if (cardEntity == null) return;
        
        cardEntity.ContextSentence = card.ContextSentence;
        cardEntity.Translation.Language = card.Translation.Language;
        cardEntity.Translation.Translation = card.Translation.Translation;
        cardEntity.Translation.Definition = card.Translation.Definition;
        cardEntity.Word.Value = card.Word.Value;
        cardEntity.Word.Language = card.Word.Language;
        
        await _db.SaveChangesAsync();
    }
    public async Task UpdateCardReviewAsync(WorditionCard card)
    {
        await _db.Cards
            .Where(c => c.Id == card.Id &&  c.UserId == card.UserId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.State, card.State)
                .SetProperty(c=> c.Step, card.Step)
                .SetProperty(c => c.Due, card.Due)
                .SetProperty(c => c.Stability, card.Stability)
                .SetProperty(c => c.Difficulty, card.Difficulty)
                .SetProperty(c => c.LastReviewedAt, card.LastReviewedAt)
                .SetProperty(c => c.Intervals.Again, card.Intervals.Again)
                .SetProperty(c => c.Intervals.Hard, card.Intervals.Hard)
                .SetProperty(c => c.Intervals.Good, card.Intervals.Good)
                .SetProperty(c => c.Intervals.Easy, card.Intervals.Easy)
            );
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