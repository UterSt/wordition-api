using System.ComponentModel.DataAnnotations;
using Wordition.Domain.Enums;

namespace Wordition.Domain.Entities;

public class Word
{
    public int Id { get; set; }
    [Required]
    [MaxLength(20)]
    public string Value { get; set; }
    public Language Language { get; set; }
}