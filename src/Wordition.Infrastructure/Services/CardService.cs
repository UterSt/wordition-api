using FSRS.Core.Interfaces;
using Wordition.Application.DTO;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Application.Interfaces.Services;
using Wordition.Domain.Exceptions;

namespace Wordition.Infrastructure.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly ISchedulerFactory  _schedulerFactory;

    public CardService(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }
    
    public async Task<List<CardResponse>> GetAllCardAsync(Guid userId)
    {
        var response = await _cardRepository.GetCardsAsync(userId);
        var result  = response
            .Select(card => card.ToResponse())
            .ToList();
        return result;
    }

    public async Task<CardResponse> GetCardAsync(Guid userId, Guid cardId)
    {
        var response = await _cardRepository.GetCardAsync(userId, cardId);
        if (response == null)
            throw new NotFoundException("Card",  cardId);
        return response.ToResponse();
    }

    public async Task<CardResponse> AddCardAsync(CardRequest cardRequest, Guid userId)
    {
        var word = cardRequest.ToWord();
        var translation = cardRequest.ToWordTranslation(word);
        var worditionCard = cardRequest.ToWorditionCard(translation, word, userId);
        await _cardRepository.AddCardAsync(worditionCard);
        return worditionCard.ToResponse();
    }

    public async Task UpdateCardAsync(CardRequest cardRequest, Guid userId, Guid cardId)
    {
        var card = await _cardRepository.GetCardAsync(userId, cardId);
        if (card == null)
            throw new NotFoundException("Card", cardId);
        var word = cardRequest.ToWord();
        var translation = cardRequest.ToWordTranslation(word);
        card.UpdateFromRequest(cardRequest, translation, word);
        await _cardRepository.UpdateCardAsync(card);
    }

    public async Task DeleteCardAsync(Guid userId, Guid cardId)
    {
        var card = await _cardRepository.GetCardAsync(userId, cardId);
        if (card == null)
            throw new NotFoundException("Card", cardId);
        await _cardRepository.DeleteCardAsync(userId, cardId);
    }

    public async Task<CardResponse> ReviewCardAsync(CardReviewRequest cardReviewRequest, Guid userId, Guid cardId)
    {
        var worditionCard = await _cardRepository.GetCardAsync(userId, cardId);
        if (worditionCard == null)
            throw new NotFoundException("Card", cardId);
        var card = worditionCard.ToFsrsCard();
        var scheduler = _schedulerFactory.CreateScheduler();
        var (updateCard, reviewLog) = scheduler.ReviewCard(card, cardReviewRequest.Rating, DateTime.UtcNow, cardReviewRequest.ReviewDuration);
        worditionCard.UpdateFromFsrs(updateCard);
        await _cardRepository.UpdateCardAsync(worditionCard);
        var log = reviewLog.ToWorditionReviewLog(worditionCard);
        await _cardRepository.AddReviewLogAsync(log);
        return worditionCard.ToResponse();
    }
}