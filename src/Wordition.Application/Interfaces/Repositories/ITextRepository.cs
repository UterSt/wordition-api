using Wordition.Domain.Entities;

namespace Wordition.Application.Interfaces.Repositories;

public interface ITextRepository
{
    public Task<List<Text>> GetTextsAsync(Guid userId);
    public Task<Text?> GetTextAsync(Guid userId, Guid textId);
    public Task AddTextAsync(Text text);
    public Task UpdateTextAsync(Text text);
    public Task DeleteTextAsync(Guid userId, Guid textId);
}