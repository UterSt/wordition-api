using Wordition.Application.DTO;
using Wordition.Application.DTO.Texts;
using Wordition.Application.Interfaces.Repositories;
using Wordition.Application.Interfaces.Services;
using Wordition.Domain.Entities;
using Wordition.Domain.Exceptions;

namespace Wordition.Application.Services;

public class TextService : ITextService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public TextService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<TextResponse>> GetAllTextAsync(Guid userId)
    {
        var texts = await _unitOfWork.Text.GetTextsAsync(userId);
        return texts.Select(t => t.ToResponse()).ToList();
    }

    public async Task<TextResponse> GetTextAsync(Guid userId, Guid textId)
    {
        var text = await _unitOfWork.Text.GetTextAsync(userId, textId);
        if (text == null)
            throw new NotFoundException("Text", textId);
        var tokenizerText = TextTokenizer.Tokenize(text.Content);
        var response = text.ToResponse();
        response.Tokens = tokenizerText;
        return response;
    }

    public async Task<TextResponse> AddTextAsync(TextRequest textRequest, Guid userId)
    {
        var text = textRequest.ToEntity(DateTime.UtcNow, userId);
        await _unitOfWork.Text.AddTextAsync(text);
        await _unitOfWork.SaveAsync();
        return text.ToResponse();
    }

    public async Task UpdateTextAsync(TextRequest textRequest, Guid userId, Guid textId)
    {
        var text = await _unitOfWork.Text.GetTextAsync(userId, textId);
        if (text == null)
            throw new NotFoundException("Text", textId);
        text = textRequest.ToEntity(text.CreatedAt, userId);
        text.Id = textId;
        await _unitOfWork.Text.UpdateTextAsync(text);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteTextAsync(Guid userId, Guid textId)
    {
        var text = await _unitOfWork.Text.GetTextAsync(userId, textId);
        if (text == null)
            throw new NotFoundException("Text", textId);
        await _unitOfWork.Text.DeleteTextAsync(userId, textId);
        await _unitOfWork.SaveAsync();
    }
}