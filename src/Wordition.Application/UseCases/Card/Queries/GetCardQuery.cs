using MediatR;
using Wordition.Application.DTO.Cards;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Domain.Exceptions;

namespace Wordition.Application.UseCases.Card.Queries;

public record GetCardQuery(Guid UserId, Guid CardId) : IRequest<CardResponse>;

public class GetCardQueryHandler : IRequestHandler<GetCardQuery, CardResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetCardQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CardResponse> Handle(GetCardQuery request, CancellationToken cancellationToken)
    {
        var response = await _unitOfWork.Card.GetCardAsync(request.UserId, request.CardId);
        if (response == null)
            throw new NotFoundException("Card",  request.CardId);
        return response.ToResponse();
    }
}