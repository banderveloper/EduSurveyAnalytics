using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Application.Interfaces;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities;

namespace EduSurveyAnalytics.Application.Services;

public class FormService(IApplicationDbContext context, IDateTimeProvider dateTimeProvider) : IFormService
{
    public async Task<Result<None>> CreateForm(Guid ownerId, string title, IEnumerable<FormFieldCreationDataDTO> formFields)
    {
        var newFormId = Guid.NewGuid();
        var newForm = new Form
        {
            Id = newFormId,
            OwnerId = ownerId,
            Title = title,
            FormFields = formFields.Select(ff => new FormField
            {
                Title = ff.Title,
                Order = ff.Order,
                FormId = newFormId,
                Constraints = ff.Constraints
            }).ToList(),
            CreatedAt = dateTimeProvider.Now,
            UpdatedAt = dateTimeProvider.Now
        };

        context.Forms.Add(newForm);
        await context.SaveChangesAsync();

        return Result<None>.Success();
    }
}