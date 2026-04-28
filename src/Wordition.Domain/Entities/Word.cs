using Wordition.Domain.Enums;

namespace Wordition.Domain.Entities;

public class Word
{
    public int Id { get; set; }
    public string Value { get; set; }
    public Language Language { get; set; }
}