using Wordition.Domain.Enums;

namespace Wordition.Domain.Entities;

public class Token
{
    public string Content { get; set; }
    public WordTokenType Type { get; set; }
    public int Position { get; set; }
}