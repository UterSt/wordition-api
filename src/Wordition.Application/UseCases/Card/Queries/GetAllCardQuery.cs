using MediatR;
using Wordition.Application.DTO.Cards;
using Wordition.Application.Interfaces.Repositories;

namespace Wordition.Application.UseCases.Card.Queries;

public record GetAllCardQuery(Guid UserId) :  IRequest<List<CardResponse>>;

public class GetAllCardQueryHandler : IRequestHandler<GetAllCardQuery, List<CardResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllCardQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<CardResponse>> Handle(GetAllCardQuery request, CancellationToken cancellationToken)
    {
        var response = await _unitOfWork.Card.GetCardsAsync(request.UserId);
        var result  = response
            .Select(card => card.ToResponse())
            .ToList();
        return result;
    }
}