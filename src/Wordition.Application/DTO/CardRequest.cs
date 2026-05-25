using Wordition.Domain.Enums;

namespace Wordition.Application.DTO;

public class CardRequest
{
    public required string Word { get; set; }
    public string? ContextSentence { get; set; }
    public required string Translation { get; set; }
    public string? Definition {get; set; }
    public Language Language { get; set; }
}