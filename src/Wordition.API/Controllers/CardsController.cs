using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordition.Application.DTO;
using Wordition.Application.DTO.Cards;
using Wordition.Application.Interfaces.Services;
using Wordition.Application.UseCases.Card.Commands;
using Wordition.Application.UseCases.Card.Queries;

namespace Wordition.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class CardsController : ControllerBase
{
    //private readonly ICardService _cardService;
    private readonly IMediator _mediator;

    public CardsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        //var cards = await _cardService.GetAllCardAsync(GetUserId());
        var cards = await _mediator.Send(new GetAllCardQuery(GetUserId()));
        return Ok(cards);
    }

    [HttpGet("{cardId}")]
    public async Task<IActionResult> GetCard(Guid cardId)
    {
        //var card = await _cardService.GetCardAsync(GetUserId(), cardId);
        var card = await _mediator.Send(new GetCardQuery(GetUserId(), cardId));
        return Ok(card);
    }

    [HttpPost()]
    public async Task<IActionResult> AddCard(CardRequest cardRequest)
    {
        //var response = await _cardService.AddCardAsync(cardRequest, GetUserId());
        var response = await _mediator.Send(new AddCardCommand(cardRequest, GetUserId()));
        return CreatedAtAction(nameof(GetCard), new {cardId = response.Id}, response);
    }

    [HttpPut("{cardId}")]
    public async Task<IActionResult> UpdateCard(CardRequest cardRequest,  Guid cardId)
    {
        //await _cardService.UpdateCardAsync(cardRequest, GetUserId(), cardId);
        await _mediator.Send(new UpdateCardCommand(cardRequest, GetUserId(), cardId));
        return Ok();
    }

    [HttpDelete("{cardId}")]
    public async Task<IActionResult> DeleteCard(Guid cardId)
    {
        //await _cardService.DeleteCardAsync(GetUserId(), cardId);
        await _mediator.Send(new DeleteCardCommand(GetUserId(), cardId));
        return Ok();
    }

    [HttpPut("review/{cardId}")]
    public async Task<IActionResult> ReviewCard(CardReviewRequest cardReviewRequest, Guid cardId)
    {
       //var response = await _cardService.ReviewCardAsync(cardReviewRequest, GetUserId(), cardId);
       var response = await _mediator.Send(new ReviewCardCommand(cardReviewRequest, GetUserId(), cardId));
       return Ok(response);
    }

    [HttpGet("due")]
    public async Task<IActionResult> GetDue()
    {
        //var response = await _cardService.GetAllDueCardsAsync(GetUserId());
        var response = await _mediator.Send(new GetAllDueCardsQuery(GetUserId()));
        return Ok(response);
    }
    
    private Guid GetUserId()
    {
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        return userId;
    }
}