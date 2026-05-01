using System.ComponentModel.DataAnnotations;
using Wordition.Domain.ValueObjects;

namespace Wordition.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Login { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    public Email? Email { get; set; }
    public List<Card> Cards { get; set; } = new();
    public List<Text> Texts { get; set; } = new();
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}