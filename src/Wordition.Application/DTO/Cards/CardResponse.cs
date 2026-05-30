using Wordition.Domain.Entities;
using Wordition.Domain.Enums;

namespace Wordition.Application.DTO.Cards;

public class CardResponse
{
    public Guid Id { get; set; }
    public required string Word { get; set; }
    public string? ContextSentence { get; set; }
    public required string Translation { get; set; }
    public string? Definition {get; set; }
    public Language Language { get; set; }
    public DateTime Due {get; set; }
    public DateTime CreatedAt { get; set; }
    public required LengthRepetitionIntervals Intervals { get; set; }
}