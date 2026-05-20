using System.ComponentModel.DataAnnotations;

namespace Wordition.Domain.ValueObjects;

public record Email()
{
    [EmailAddress]
    public string Value { get; set; }
};