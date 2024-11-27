using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Application.Interfaces;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduSurveyAnalytics.Application.Services;

public class AnswerService(IApplicationDbContext context, IDateTimeProvider dateTimeProvider) : IAnswerService
{
    public async Task<Result<None>> CreateAnswerAsync(Guid answererId, Guid formId,
        IEnumerable<FieldAnswerCreationDataDTO> fieldAnswers)
    {
        var existingForm = await context.Forms.FindAsync(formId);
        if (existingForm is null)
            return Result<None>.Error(ErrorCode.FormNotFound);

        var newAnswer = new Answer
        {
            Id = Guid.NewGuid(),
            FormId = formId,
            UserId = answererId,
            CreatedAt = dateTimeProvider.Now
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

    public async Task<Result<FormAnswersPresentationDTO?>> GetFormAnswers(Guid formId)
    {
        var form = await context.Forms
            .Include(form => form.Answers)
            .Include(form => form.FormFields)
            .ThenInclude(formField => formField.FieldAnswers)
            .ThenInclude(fieldAnswer => fieldAnswer.Answer)
            .ThenInclude(answer => answer.User)
            .FirstOrDefaultAsync(form => form.Id == formId);

        if (form is null)
            return Result<FormAnswersPresentationDTO?>.Error(ErrorCode.FormNotFound);

        var formAnswersPresentationDto = new FormAnswersPresentationDTO
        {
            FormTitle = form.Title,
            FormFields = form.FormFields.Select(formField => new FormFieldAnswersPresentationDTO
            {
                Title = formField.Title,
                Answers = formField.FieldAnswers.Select(fieldAnswer => new FormFieldAnswerPresentationDTO
                {
                    Value = fieldAnswer.Value,
                    AnsweredAt = fieldAnswer.Answer.CreatedAt,
                    UserName = string.Join(" ", fieldAnswer.Answer.User.LastName, fieldAnswer.Answer.User.FirstName,
                        fieldAnswer.Answer.User.MiddleName)
                })
            }),
            AnswersCount = form.Answers.Count
        };

        return Result<FormAnswersPresentationDTO?>.Success(formAnswersPresentationDto);
    }
}