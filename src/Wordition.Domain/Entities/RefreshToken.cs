namespace Wordition.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow.AddDays(30);
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}