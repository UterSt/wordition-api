namespace Wordition.Application.DTO.Auth;

public record AuthResponse()
{
    public string? Token { get; init; }
};