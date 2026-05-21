using System.Text.Json.Serialization;

namespace Wordition.Infrastructure.Models;

public class MyMemoryResponseData
{
    [JsonPropertyName("translatedText")]
    public required string TranslatedText {get; set;}
    [JsonPropertyName("match")]
    public double Match {get; set;}
}