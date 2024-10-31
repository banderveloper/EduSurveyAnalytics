using EduSurveyAnalytics.Application.DTO;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IFormService
{
    Task<Result<None>> CreateForm(Guid ownerId, string title, IEnumerable<FormFieldCreationDataDTO> formFields);
    Task<Result<FormPresentationDTO?>> GetFormPresentationById(Guid formId);
}