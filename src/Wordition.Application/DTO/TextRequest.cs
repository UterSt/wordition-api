using System.ComponentModel.DataAnnotations;
using Wordition.Domain.Enums;

namespace Wordition.Application.DTO;

public class TextRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; set; }
    [MaxLength(1000000)]
    public string Text { get; set; }
    public bool IsPublic {get; set; }
    public Language Language { get; set; }
}