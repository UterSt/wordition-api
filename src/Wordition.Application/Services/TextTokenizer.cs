using System.Text.RegularExpressions;
using Wordition.Domain.Entities;
using Wordition.Domain.Enums;

namespace Wordition.Application.Services;

public static class TextTokenizer
{
    private static readonly Regex _pattern = new (@"(?<Number>\d+)|(?<Word>\w+[`-]?\w+)|(?<Space>\s+)|(?<Point>[,.!?—])");

    public static List<Token> Tokenize(string text)
    {
        List<Token> tokens = new List<Token>();

        foreach (Match match in _pattern.Matches(text))
        {
            var type =  match.Groups["Number"].Success ? WordTokenType.Number
                    : match.Groups["Word"].Success ? WordTokenType.Word
                    : match.Groups["Space"].Success ? WordTokenType.Space
                    : WordTokenType.Point;
            
            tokens.Add(new Token()
            {
                Content = match.Value,
                Type = type,
                Position = match.Index,
            });
        }
        return tokens;
    }
}