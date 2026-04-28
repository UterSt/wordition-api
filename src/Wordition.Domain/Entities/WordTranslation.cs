using Wordition.Domain.Enums;

namespace Wordition.Domain.Entities;

public class WordTranslation
{
    public int Id { get; set; }
    public Language Language { get; set; }
    public string Translation { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; }
    public string? Definition {get; set; }
}