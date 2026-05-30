using Wordition.Application.DTO;
using Wordition.Application.DTO.Cards;

namespace Wordition.Application.Interfaces.Services;

public interface ICardService
{
    public Task<List<CardResponse>> GetAllCardAsync(Guid userId);
    public Task<List<CardResponse>> GetAllDueCardsAsync(Guid userId);
    public Task<CardResponse> GetCardAsync(Guid userId, Guid cardId);
    public Task<CardResponse> AddCardAsync(CardRequest cardRequest, Guid userId);
    public Task UpdateCardAsync(CardRequest cardRequest, Guid userId, Guid cardId);
    public Task DeleteCardAsync(Guid userId, Guid cardId);
    public Task<CardResponse> ReviewCardAsync(CardReviewRequest cardReviewRequestRequest, Guid userId, Guid cardId);
}