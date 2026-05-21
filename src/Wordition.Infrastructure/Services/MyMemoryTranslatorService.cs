using System.Net.Http.Json;
using Wordition.Application.DTO;
using Wordition.Application.Interfaces.Services;
using Wordition.Domain.Exceptions;
using Wordition.Infrastructure.Models;

namespace Wordition.Infrastructure.Services;

public class MyMemoryTranslatorService(HttpClient httpClient) : ITranslatorService
{
    public async Task<TranslationResponse?> GetTranslationAsync(TranslationRequest request)
    {
        var response = await httpClient.GetFromJsonAsync<MyMemoryResponse>($"/get?q={request.Word}&langpair={request.OriginalLanguage.GetReduction()}|{request.TargetLanguage.GetReduction()}");
        if (response == null)
            throw new BadGatewayException("MyMemoryTranslation");
        var result = new TranslationResponse()
        {
            Translation = response.ResponseData.TranslatedText
        };
        return result;
    }
}