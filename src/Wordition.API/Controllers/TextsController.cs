using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordition.Application.DTO.Texts;
using Wordition.Application.Interfaces.Services;

namespace Wordition.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class TextsController : ControllerBase
{
    private readonly ITextService _textService;
    
    public TextsController(ITextService  textService)
    {
        _textService = textService;
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var texts = await _textService.GetAllTextAsync(GetUserId());
        return Ok(texts);
    }

    [HttpGet("{textId}")]
    public async Task<IActionResult> GetTokenText(Guid textId)
    {
        var tokenizerText = await _textService.GetTextAsync(GetUserId(), textId);
        return Ok(tokenizerText);
    }

    [HttpPost()]
    public async Task<IActionResult> AddText(TextRequest request)
    {
        var response = await _textService.AddTextAsync(request,  GetUserId());
        return CreatedAtAction(nameof(GetTokenText), new {textId = response.Id},  response);
    }

    [HttpPut("{textId}")]
    public async Task<IActionResult> UpdateText(Guid textId, TextRequest request)
    {
        await _textService.UpdateTextAsync( request, GetUserId(), textId);
        return Ok();
    }

    [HttpDelete("{textId}")]
    public async Task<IActionResult> DeleteTextAsync(Guid textId)
    {
        await _textService.DeleteTextAsync(GetUserId(), textId);
        return Ok();
    }

    private Guid GetUserId()
    {
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        return userId;
    }
}