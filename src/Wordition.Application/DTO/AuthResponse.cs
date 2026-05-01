namespace Wordition.Application.DTO;

public record AuthResponse()
{
    public string? Token { get; init; }
};