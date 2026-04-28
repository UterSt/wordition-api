using Wordition.Domain.Enums;

namespace Wordition.Domain.Entities;

public class Text
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Language Language { get; set; }
    public bool IsPublic {get; set; }
    public DateTime CreatedAt { get; set; }
}