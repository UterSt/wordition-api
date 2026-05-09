using Wordition.Domain.Enums;

namespace Wordition.Application.DTO;

public class TextRequest
{
    public string Title { get; set; }
    public string Text { get; set; }
    public bool IsPublic {get; set; }
    public Language Language { get; set; }
}