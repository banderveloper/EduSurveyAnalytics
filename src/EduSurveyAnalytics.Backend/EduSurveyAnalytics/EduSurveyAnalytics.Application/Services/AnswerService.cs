using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Application.Interfaces;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities;

namespace EduSurveyAnalytics.Application.Services;

public class AnswerService(IApplicationDbContext context) : IAnswerService
{
    public async Task<Result<None>> CreateAnswerAsync(Guid answererId, Guid formId, IEnumerable<FieldAnswerCreationDataDTO> fieldAnswers)
    {
        var existingForm = await context.Forms.FindAsync(formId);
        if (existingForm is null)
            return Result<None>.Error(ErrorCode.FormNotFound);
        
        var newAnswer = new Answer
        {
            Id = Guid.NewGuid(),
            FormId = formId,
            UserId = answererId
        };

        newAnswer.FieldAnswers = fieldAnswers.Select(fa => new FieldAnswer
        {
            Value = fa.Value,
            FormFieldId = fa.FormFieldId,
            AnswerId = newAnswer.Id
        }).ToList();

        context.Answers.Add(newAnswer);
        await context.SaveChangesAsync();

        return Result<None>.Success();
    }
}