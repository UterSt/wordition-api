using Wordition.Domain.ValueObjects;

namespace Wordition.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public Email? Email { get; set; }
    public List<Card> Cards { get; set; }
    public List<Text> Texts { get; set; }
    public DateTime RegisteredAt { get; set; }
}