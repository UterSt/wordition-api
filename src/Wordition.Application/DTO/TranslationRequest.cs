using Wordition.Domain.Enums;

namespace Wordition.Application.DTO;

public class TranslationRequest
{
    public required string Word { get; set; }
    public string? Context { get; set; }
    public required Language OriginalLanguage { get; set; }
    public required Language TargetLanguage { get; set; }
}