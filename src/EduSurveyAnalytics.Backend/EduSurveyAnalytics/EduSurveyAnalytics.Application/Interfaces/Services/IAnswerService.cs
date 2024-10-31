using EduSurveyAnalytics.Application.DTO;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IAnswerService
{
    Task<Result<None>> CreateAnswerAsync(Guid answererId, Guid formId, IEnumerable<FieldAnswerCreationDataDTO> fieldAnswers);
}