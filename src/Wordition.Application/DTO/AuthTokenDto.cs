namespace Wordition.Application.DTO;

public record AuthTokenDto()
{
    public string? JwtToken { get; init; }
    public string? RefreshToken { get; init; }
};