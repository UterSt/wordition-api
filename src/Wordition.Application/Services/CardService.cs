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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchedulerFactory  _schedulerFactory;

    public CardService(IUnitOfWork unitOfWork, ISchedulerFactory schedulerFactory)
    {
        _unitOfWork = unitOfWork;
        _schedulerFactory = schedulerFactory;
    }
    
    public async Task<List<CardResponse>> GetAllCardAsync(Guid userId)
    {
        var response = await _unitOfWork.Card.GetCardsAsync(userId);
        var result  = response
            .Select(card => card.ToResponse())
            .ToList();
        return result;
    }

    public async Task<List<CardResponse>> GetAllDueCardsAsync(Guid userId)
    {
        var response = await _unitOfWork.Card.GetDueCardsAsync(userId);
        var result  = response
            .Select(card => card.ToResponse())
            .ToList();
        return result;
    }

    public async Task<CardResponse> GetCardAsync(Guid userId, Guid cardId)
    {
        var response = await _unitOfWork.Card.GetCardAsync(userId, cardId);
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
        await _unitOfWork.Card.AddCardAsync(worditionCard);
        await _unitOfWork.SaveAsync();
        return worditionCard.ToResponse();
    }

    public async Task UpdateCardAsync(CardRequest cardRequest, Guid userId, Guid cardId)
    {
        var card = await _unitOfWork.Card.GetCardAsync(userId, cardId);
        if (card == null)
            throw new NotFoundException("Card", cardId);
        var word = cardRequest.ToWord();
        var translation = cardRequest.ToWordTranslation(word);
        card.UpdateFromRequest(cardRequest, translation, word);
        await _unitOfWork.Card.UpdateCardContentAsync(card);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCardAsync(Guid userId, Guid cardId)
    {
        var card = await _unitOfWork.Card.GetCardAsync(userId, cardId);
        if (card == null)
            throw new NotFoundException("Card", cardId);
        await _unitOfWork.Card.DeleteCardAsync(userId, cardId);
        await _unitOfWork.SaveAsync();
    }

    public async Task<CardResponse> ReviewCardAsync(CardReviewRequest cardReviewRequest, Guid userId, Guid cardId)
    {
        var worditionCard = await _unitOfWork.Card.GetCardAsync(userId, cardId);
        if (worditionCard == null)
            throw new NotFoundException("Card", cardId);
        var card = worditionCard.ToFsrsCard();
        var scheduler = _schedulerFactory.CreateScheduler();
        var (updateCard, reviewLog) = scheduler.ReviewCard(card, cardReviewRequest.Rating, DateTime.UtcNow, cardReviewRequest.ReviewDuration);
        worditionCard.UpdateFromFsrs(updateCard);
        worditionCard.Intervals = GetRepetitionIntervals(updateCard);
        await _unitOfWork.Card.UpdateCardReviewAsync(worditionCard);
        var log = reviewLog.ToWorditionReviewLog(worditionCard);
        await _unitOfWork.Card.AddReviewLogAsync(log);
        await _unitOfWork.SaveAsync();
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