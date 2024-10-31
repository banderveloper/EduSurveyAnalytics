using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Application.Interfaces;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduSurveyAnalytics.Application.Services;

public class FormService(IApplicationDbContext context, IDateTimeProvider dateTimeProvider) : IFormService
{
    public async Task<Result<None>> CreateFormAsync(Guid ownerId, string title,
        IEnumerable<FormFieldCreationDataDTO> formFields)
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

    public async Task<Result<FormPresentationDTO?>> GetFormPresentationByIdAsync(Guid formId)
    {
        var formEntity = await context.Forms
            .Include(f => f.FormFields)
            .Include(f => f.Owner)
            .FirstOrDefaultAsync(f => f.Id == formId);

        if (formEntity is null)
            return Result<FormPresentationDTO?>.Success(null);

        var formPresentation = new FormPresentationDTO
        {
            Id = formEntity.Id,
            OwnerId = formEntity.OwnerId,
            OwnerName = string.Concat(formEntity.Owner.LastName, " ", formEntity.Owner.FirstName, " ", formEntity.Owner.MiddleName ?? ""),
            OwnerPost = formEntity.Owner.Post,
            Fields = formEntity.FormFields.Select(ff => new FormFieldPresentationDTO
            {
                Id = ff.Id,
                Order = ff.Order,
                Constraints = ff.Constraints,
                Title = ff.Title
            }).OrderBy(ff => ff.Order)
        };

        return Result<FormPresentationDTO?>.Success(formPresentation);
    }
}