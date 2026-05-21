namespace Wordition.Application.DTO;

public class TranslationResponse
{
    public required string Translation {get; set;}
    public string? Definition {get; set;}
    public string? WordType {get; set;}
}