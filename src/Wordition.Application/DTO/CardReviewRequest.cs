using FSRS.Core.Enums;

namespace Wordition.Application.DTO;

public class CardReviewRequest
{
    public Rating Rating { get; set; }
    public int ReviewDuration { get; set; }
}