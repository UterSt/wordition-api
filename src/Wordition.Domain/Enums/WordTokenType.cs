using System.Text.Json.Serialization;

namespace Wordition.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WordTokenType
{
    Word,
    Number,
    Space,
    Point
}