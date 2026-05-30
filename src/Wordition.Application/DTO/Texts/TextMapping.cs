using Wordition.Domain.Entities;

namespace Wordition.Application.DTO.Texts;

public static class TextMapping
{
    public static TextResponse ToResponse(this Text text) => new()
    {
        Id = text.Id,
        Title = text.Title,
        CreatedAt = text.CreatedAt,
        IsPublic = text.IsPublic,
        Language = text.Language,
    };

    public static Text ToEntity(this TextRequest request, Guid userId) => new()
    {
        Title = request.Title,
        Content = request.Text,
        IsPublic = request.IsPublic,
        Language = request.Language,
        UserId = userId
    };
}