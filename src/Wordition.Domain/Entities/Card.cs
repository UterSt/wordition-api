namespace Wordition.Domain.Entities;

public class Card
{
    public int Id { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public int WordTranslationId { get; set; }
    public WordTranslation Translation { get; set; }
    public string ContextSentence {get; set; }
    public DateTime LastReviewedAt { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}