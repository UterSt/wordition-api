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
        if (response.JwtToken == null || response.RefreshToken == null)
        {
            return Unauthorized();
        }
        SetCookie(response.RefreshToken);
        return Ok(response.JwtToken);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            DeleteCookie();
            return Unauthorized();
        }
        var response = await _authService.RefreshAsync(refreshToken);
        if (response.JwtToken == null || response.RefreshToken == null)
            return Unauthorized();
        DeleteCookie();
        SetCookie(response.RefreshToken);
        return Ok(response.JwtToken);
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _authService.LogoutAsync(refreshToken);
        }
        DeleteCookie();
        return Ok("Logged out");
    }

    private void SetCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(30),
            Path = "/"
        };
        Response.Cookies.Append("refreshToken",  refreshToken, cookieOptions);
    }
    private void DeleteCookie()
    {
        Response.Cookies.Delete("refreshToken", new CookieOptions()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(-1),
            Path = "/"
        });
    }
}