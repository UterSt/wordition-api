using Wordition.Application.DTO;
using Wordition.Domain.Entities;

namespace Wordition.Application.Interfaces.Repositories;

public interface ICardRepository
{
    public Task<List<WorditionCard>> GetCardsAsync(Guid userId);
    public Task<WorditionCard?> GetCardAsync(Guid userId, Guid cardId);
    public Task AddCardAsync(WorditionCard card);
    public Task UpdateCardAsync(WorditionCard card);
    public Task DeleteCardAsync(Guid userId, Guid cardId);
    public Task AddReviewLogAsync(CardReviewLog reviewLog);
    public Task<List<CardReviewLog>> GetCardReviewLogsAsync(Guid userId,  Guid cardId);
}