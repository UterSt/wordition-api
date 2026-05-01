using Microsoft.AspNetCore.Mvc;
using Wordition.Application.DTO;
using Wordition.Application.Interfaces.Services;

namespace Wordition.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Registration([FromBody] AuthRequest request)
    {
        await _authService.RegisterAsync(request.Login, request.Password, request.Email);
        return Ok("Account created");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var response = await _authService.LoginAsync(request.Login, request.Password);
        if (response.Token == null)
        {
            return Unauthorized();
        }
        return Ok(response);
    }
}