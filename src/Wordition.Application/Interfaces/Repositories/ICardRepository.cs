using Wordition.Application.DTO;
using Wordition.Domain.Entities;

namespace Wordition.Application.Interfaces.Repositories;

public interface ICardRepository
{
    public Task<List<WorditionCard>> GetCardsAsync(Guid userId);
    public Task<WorditionCard> GetCardAsync(Guid userId, Guid cardId);
    public Task AddCardAsync(CardRequest card);
    public Task UpdateCardAsync(CardRequest card);
    public Task DeleteCardAsync(Guid userId, Guid cardId);
}