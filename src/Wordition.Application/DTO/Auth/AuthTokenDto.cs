namespace Wordition.Application.DTO.Auth;

public record AuthTokenDto()
{
    public string? JwtToken { get; init; }
    public string? RefreshToken { get; init; }
};