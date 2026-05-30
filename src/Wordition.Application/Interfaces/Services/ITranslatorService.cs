using Wordition.Application.DTO;
using Wordition.Application.DTO.Translations;

namespace Wordition.Application.Interfaces.Services;

public interface ITranslatorService
{
    public Task<TranslationResponse> GetTranslationAsync(TranslationRequest request);
}