using FSRS.Core.Enums;

namespace Wordition.Domain.Entities;

public class WorditionCard
{
    public Guid Id { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public int WordTranslationId { get; set; }
    public WordTranslation Translation { get; set; }
    public string? ContextSentence {get; set; }
    public DateTime? LastReviewedAt { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public State State { get; set; }
    public int? Step { get; set; }
    public double? Stability {get; set; }
    public double? Difficulty { get; set; }
    public DateTime Due {get; set; }
    public required LengthRepetitionIntervals Intervals {get; set; }
}