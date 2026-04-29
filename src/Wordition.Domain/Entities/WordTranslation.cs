using System.ComponentModel.DataAnnotations;
using Wordition.Domain.Enums;

namespace Wordition.Domain.Entities;

public class WordTranslation
{
    public int Id { get; set; }
    public Language Language { get; set; }
    [Required]
    [MaxLength(30)]
    public string Translation { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; }
    [MaxLength(100)]
    public string? Definition {get; set; }
}