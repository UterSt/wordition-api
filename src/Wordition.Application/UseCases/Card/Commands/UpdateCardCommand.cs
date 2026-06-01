using MediatR;
using Wordition.Application.DTO.Cards;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Domain.Exceptions;

namespace Wordition.Application.UseCases.Card.Commands;

public record UpdateCardCommand(CardRequest CardRequest, Guid UserId, Guid CardId) :  IRequest;

public class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Card.GetCardAsync(request.UserId, request.CardId);
        if (card == null)
            throw new NotFoundException("Card", request.CardId);
        var word = request.CardRequest.ToWord();
        var translation = request.CardRequest.ToWordTranslation(word);
        card.UpdateFromRequest(request.CardRequest, translation, word);
        await _unitOfWork.Card.UpdateCardContentAsync(card);
        await _unitOfWork.SaveAsync();
    }
}