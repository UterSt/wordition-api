using System.ComponentModel.DataAnnotations;
using Wordition.Domain.Enums;

namespace Wordition.Domain.Entities;

public class Text
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    [Required]
    [MaxLength(20)]
    public string Title { get; set; }
    public string Content { get; set; }
    public Language Language { get; set; }
    public bool IsPublic {get; set; }
    public DateTime CreatedAt { get; set; }
}