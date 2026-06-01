using MediatR;
using Wordition.Application.DTO.Cards;
using Wordition.Application.Interfaces.Repositories;

namespace Wordition.Application.UseCases.Card.Queries;

public record GetAllDueCardsQuery(Guid UserId) :  IRequest<List<CardResponse>>;

public class GetAllDueCardsQueryHandler : IRequestHandler<GetAllDueCardsQuery, List<CardResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllDueCardsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<CardResponse>> Handle(GetAllDueCardsQuery request, CancellationToken cancellationToken)
    {
        var response = await _unitOfWork.Card.GetDueCardsAsync(request.UserId);
        var result  = response
            .Select(card => card.ToResponse())
            .ToList();
        return result;
    }
}