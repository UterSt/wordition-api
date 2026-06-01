using MediatR;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Domain.Exceptions;

namespace Wordition.Application.UseCases.Card.Commands;

public record DeleteCardCommand(Guid UserId, Guid CardId) :  IRequest;

public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Card.GetCardAsync(request.UserId, request.CardId);
        if (card == null)
            throw new NotFoundException("Card", request.CardId);
        await _unitOfWork.Card.DeleteCardAsync(request.UserId, request.CardId);
        await _unitOfWork.SaveAsync();
    }
}