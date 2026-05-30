using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordition.Application.DTO;
using Wordition.Application.DTO.Translations;
using Wordition.Application.Interfaces.Services;

namespace Wordition.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/v1")]
public class TranslationController : ControllerBase
{
    private ITranslatorService _translatorService;
    
    public TranslationController(ITranslatorService  translatorService)
    {
        _translatorService = translatorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTranslation([FromQuery] TranslationRequest request)
    {
        var result = await _translatorService.GetTranslationAsync(request);
        return Ok(result);
    }
}