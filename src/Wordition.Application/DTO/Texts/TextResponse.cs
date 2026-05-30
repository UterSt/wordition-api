using Wordition.Domain.Entities;
using Wordition.Domain.Enums;

namespace Wordition.Application.DTO.Texts;

public record TextResponse()
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Language Language { get; set; }
    public bool IsPublic {get; set; }
    public DateTime CreatedAt { get; set; }
};