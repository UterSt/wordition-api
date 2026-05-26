using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordition.Application.DTO;
using Wordition.Application.Interfaces.Services;

namespace Wordition.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/v1")]
public class CardsController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardsController(ICardService cardService)
    {
        _cardService = cardService;
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var cards = await _cardService.GetAllCardAsync(GetUserId());
        return Ok(cards);
    }

    [HttpGet("{cardId}")]
    public async Task<IActionResult> GetCard(Guid cardId)
    {
        var card = await _cardService.GetCardAsync(GetUserId(), cardId);
        return Ok(card);
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddCard(CardRequest cardRequest)
    {
        var response = await _cardService.AddCardAsync(cardRequest, GetUserId());
        return CreatedAtAction(nameof(GetCard), new {cardId = response.Id}, response);
    }

    [HttpPut("{cardId}")]
    public async Task<IActionResult> UpdateCard(CardRequest cardRequest,  Guid cardId)
    {
        await _cardService.UpdateCardAsync(cardRequest, GetUserId(), cardId);
        return Ok();
    }

    [HttpDelete("{cardId}")]
    public async Task<IActionResult> DeleteCard(Guid cardId)
    {
        await _cardService.DeleteCardAsync(GetUserId(), cardId);
        return Ok();
    }

    [HttpPut("review/{cardId}")]
    public async Task<IActionResult> ReviewCard(CardReviewRequest cardReviewRequest, Guid cardId)
    {
       var response = await _cardService.ReviewCardAsync(cardReviewRequest, GetUserId(), cardId);
       return Ok(response);
    }
    
    private Guid GetUserId()
    {
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        return userId;
    }
}