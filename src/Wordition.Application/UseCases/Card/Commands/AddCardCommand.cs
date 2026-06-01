using FSRS.Core.Enums;
using FSRS.Core.Interfaces;
using MediatR;
using Wordition.Application.DTO.Cards;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Domain.Entities;

namespace Wordition.Application.UseCases.Card.Commands;

public record AddCardCommand(CardRequest CardRequest, Guid UserId) : IRequest<CardResponse>;

public class AddCardCommandHandler : IRequestHandler<AddCardCommand, CardResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchedulerFactory  _schedulerFactory;
    
    public AddCardCommandHandler(IUnitOfWork unitOfWork,  ISchedulerFactory schedulerFactory)
    {
        _unitOfWork = unitOfWork;
        _schedulerFactory = schedulerFactory;
    }

    public async Task<CardResponse> Handle(AddCardCommand request, CancellationToken cancellationToken)
    {
        var word = request.CardRequest.ToWord();
        var translation = request.CardRequest.ToWordTranslation(word);
        var card = new FSRS.Core.Models.Card();
        var intervals = GetRepetitionIntervals(card);
        var worditionCard = request.CardRequest.ToWorditionCard(translation, word, intervals, request.UserId);
        await _unitOfWork.Card.AddCardAsync(worditionCard);
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