using Wordition.Application.DTO;

namespace Wordition.Application.Interfaces.Services;

public interface ICardService
{
    public Task<List<CardResponse>> GetAllCardAsync(Guid userId);
    public Task<CardResponse> GetCardAsync(Guid userId, Guid cardId);
    public Task<CardResponse> AddCardAsync(CardRequest cardRequest, Guid userId);
    public Task UpdateCardAsync(CardRequest textRequest, Guid userId, Guid cardId);
    public Task DeleteCardAsync(Guid userId, Guid cardId);
}