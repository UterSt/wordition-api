using Wordition.Domain.ValueObjects;

namespace Wordition.Application.DTO;

public class AuthRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
    public Email? Email { get; set; }
}