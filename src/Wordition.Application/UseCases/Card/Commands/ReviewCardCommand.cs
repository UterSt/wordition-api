using FSRS.Core.Enums;
using FSRS.Core.Interfaces;
using MediatR;
using Wordition.Application.DTO.Cards;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Domain.Entities;
using Wordition.Domain.Exceptions;

namespace Wordition.Application.UseCases.Card.Commands;

public record ReviewCardCommand(CardReviewRequest CardReviewRequest, Guid UserId, Guid CardId) : IRequest<CardResponse>;

public class ReviewCardCommandHandler : IRequestHandler<ReviewCardCommand, CardResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchedulerFactory  _schedulerFactory;
    
    public ReviewCardCommandHandler(IUnitOfWork unitOfWork,  ISchedulerFactory schedulerFactory)
    {
        _unitOfWork = unitOfWork;
        _schedulerFactory = schedulerFactory;
    }
    
    public async Task<CardResponse> Handle(ReviewCardCommand request, CancellationToken cancellationToken)
    {
        var worditionCard = await _unitOfWork.Card.GetCardAsync(request.UserId, request.CardId);
        if (worditionCard == null)
            throw new NotFoundException("Card", request.CardId);
        var card = worditionCard.ToFsrsCard();
        var scheduler = _schedulerFactory.CreateScheduler();
        var (updateCard, reviewLog) = scheduler.ReviewCard(card, request.CardReviewRequest.Rating, DateTime.UtcNow, request.CardReviewRequest.ReviewDuration);
        worditionCard.UpdateFromFsrs(updateCard);
        worditionCard.Intervals = GetRepetitionIntervals(updateCard);
        await _unitOfWork.Card.UpdateCardReviewAsync(worditionCard);
        var log = reviewLog.ToWorditionReviewLog(worditionCard);
        await _unitOfWork.Card.AddReviewLogAsync(log);
        await _unitOfWork.SaveAsync();
        return worditionCard.ToResponse();
    }
    
    private LengthRepetitionIntervals GetRepetitionIntervals(FSRS.Core.Models.Card card)
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