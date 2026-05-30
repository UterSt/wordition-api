using FSRS.Core.Enums;
using FSRS.Core.Interfaces;
using FSRS.Core.Models;
using Wordition.Application.DTO;
using Wordition.Application.DTO.Cards;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Application.Interfaces.Services;
using Wordition.Domain.Entities;
using Wordition.Domain.Exceptions;

namespace Wordition.Application.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly ISchedulerFactory  _schedulerFactory;

    public CardService(ICardRepository cardRepository, ISchedulerFactory schedulerFactory)
    {
        _cardRepository = cardRepository;
        _schedulerFactory = schedulerFactory;
    }
    
    public async Task<List<CardResponse>> GetAllCardAsync(Guid userId)
    {
        var response = await _cardRepository.GetCardsAsync(userId);
        var result  = response
            .Select(card => card.ToResponse())
            .ToList();
        return result;
    }

    public async Task<List<CardResponse>> GetAllDueCardsAsync(Guid userId)
    {
        var response = await _cardRepository.GetDueCardsAsync(userId);
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
        var card = new Card();
        var intervals = GetRepetitionIntervals(card);
        var worditionCard = cardRequest.ToWorditionCard(translation, word, intervals, userId);
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
        await _cardRepository.UpdateCardContentAsync(card);
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
        worditionCard.Intervals = GetRepetitionIntervals(updateCard);
        await _cardRepository.UpdateCardReviewAsync(worditionCard);
        var log = reviewLog.ToWorditionReviewLog(worditionCard);
        await _cardRepository.AddReviewLogAsync(log);
        return worditionCard.ToResponse();
    }

    private LengthRepetitionIntervals GetRepetitionIntervals(Card card)
    {
        var scheduler = _schedulerFactory.CreateScheduler();
        var (updateCardAgain, _) = scheduler.ReviewCard(card, Rating.Again, DateTime.UtcNow);
        var (updateCardHard, _) = scheduler.ReviewCard(card, Rating.Hard, DateTime.UtcNow);
        var (updateCardGood, _) = scheduler.ReviewCard(card, Rating.Good, DateTime.UtcNow);
        var (updateCardEasy, _) = scheduler.ReviewCard(card, Rating.Easy, DateTime.UtcNow);
        var result = new LengthRepetitionIntervals
        {
            Again = (int)Math.Round((updateCardAgain.Due - DateTime.UtcNow).TotalMinutes),
            Hard = (int)Math.Round((updateCardHard.Due - DateTime.UtcNow).TotalMinutes),
            Good = (int)Math.Round((updateCardGood.Due - DateTime.UtcNow).TotalMinutes),
            Easy = (int)Math.Round((updateCardEasy.Due - DateTime.UtcNow).TotalMinutes),
        };
        return result;
    }
}