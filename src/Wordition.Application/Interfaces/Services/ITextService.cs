using Wordition.Application.DTO;
using Wordition.Domain.Entities;

namespace Wordition.Application.Interfaces.Services;

public interface ITextService
{
    public Task<List<TextResponse>> GetAllTextAsync(Guid userId);
    public Task<List<Token>> GetTextAsync(Guid userId, Guid textId);
    public Task<TextResponse> AddTextAsync(TextRequest textRequest, Guid userId);
    public Task UpdateTextAsync(TextRequest textRequest, Guid userId, Guid textId);
    public Task DeleteTextAsync(Guid userId, Guid textId);
}