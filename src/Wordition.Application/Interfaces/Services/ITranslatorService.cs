using Wordition.Application.DTO;

namespace Wordition.Application.Interfaces.Services;

public interface ITranslatorService
{
    public Task<TranslationResponse?> GetTranslationAsync(TranslationRequest request);
}