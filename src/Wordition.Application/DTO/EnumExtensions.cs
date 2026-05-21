using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Wordition.Domain.Enums;

namespace Wordition.Application.DTO;

public static class EnumExtensions
{
    public static string GetReduction(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        return attribute?.Value ??  value.ToString();
    }
}