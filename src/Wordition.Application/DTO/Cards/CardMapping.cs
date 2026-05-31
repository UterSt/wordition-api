using FSRS.Core.Enums;
using FSRS.Core.Models;
using Wordition.Domain.Entities;

namespace Wordition.Application.DTO.Cards;

public static class CardMapping
{
    public static CardResponse ToResponse(this WorditionCard card) => new()
    {
        Id = card.Id,
        Word = card.Word.Value,
        ContextSentence = card.ContextSentence,
        Translation = card.Translation.Translation,
        Definition = card.Translation.Definition,
        Language = card.Translation.Language,
        Due = card.Due,
        CreatedAt = card.CreatedAt,
        Intervals = card.Intervals,
        State = card.State,
        LastReviewedAt = card.LastReviewedAt,
    };

    public static WorditionCard ToWorditionCard(this CardRequest cardRequest, WordTranslation translation, Word word, LengthRepetitionIntervals intervals,
        Guid userId) => new()
    {
        Word = word,
        ContextSentence = cardRequest.ContextSentence,
        Translation = translation,
        UserId = userId,
        Due = DateTime.UtcNow,
        State = State.Learning,
        LastReviewedAt = null,
        Intervals = intervals,
    };

    public static Word ToWord(this CardRequest cardRequest) => new()
    {
        Value = cardRequest.Word,
        Language = cardRequest.SourceLanguage,
    };

    public static WordTranslation ToWordTranslation(this CardRequest cardRequest, Word word) => new()
    {
        Translation = cardRequest.Translation,
        Language = cardRequest.TranslationLanguage,
        Definition = cardRequest.Definition,
        Word = word,
    };

    public static Card ToFsrsCard(this WorditionCard worditionCard) => new()
    {
        CardId = worditionCard.Id,
        Difficulty = worditionCard.Difficulty,
        Due = worditionCard.Due,
        LastReview = worditionCard.LastReviewedAt,
        Stability = worditionCard.Stability,
        State = worditionCard.State,
        Step = worditionCard.Step,
    };

    public static void UpdateFromRequest(this WorditionCard worditionCard, CardRequest cardRequest, WordTranslation translation, Word word)
    {
        worditionCard.ContextSentence = cardRequest.ContextSentence;
        worditionCard.Word = word;
        worditionCard.Translation = translation;
    }

    public static void UpdateFromFsrs(this WorditionCard worditionCard, Card card)
    {
        worditionCard.State = card.State;
        worditionCard.Step = card.Step;
        worditionCard.Due = card.Due;
        worditionCard.Stability = card.Stability;
        worditionCard.Difficulty = card.Difficulty;
        worditionCard.LastReviewedAt = card.LastReview;
    }

    public static CardReviewLog ToWorditionReviewLog(this ReviewLog reviewLog, WorditionCard worditionCard) => new()
    {
        WorditionCardId = reviewLog.CardId,
        Rating = reviewLog.Rating,
        ReviewDate = reviewLog.ReviewDateTime,
        ReviewDuration = reviewLog.ReviewDuration,
        WorditionCard = worditionCard,
    };
}