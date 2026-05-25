using FSRS.Core.Enums;

namespace Wordition.Domain.Entities;

public class CardReviewLog
{
    public Guid Id { get; set; }
    public Guid WorditionCardId { get; set; }
    public WorditionCard WorditionCard { get; set; }
    public Rating Rating { get; set; }
    public DateTime ReviewDate { get; set; }
    public int? ReviewDuration { get; set; }
}