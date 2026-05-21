using System.Runtime.Serialization;

namespace Wordition.Domain.Enums;

public enum Language
{
    [EnumMember(Value = "ru")]
    Russian,
    [EnumMember(Value = "en")]
    English,
}