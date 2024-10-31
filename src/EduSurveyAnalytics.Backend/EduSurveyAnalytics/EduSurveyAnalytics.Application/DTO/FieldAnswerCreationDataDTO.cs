namespace EduSurveyAnalytics.Application.DTO;

public class FieldAnswerCreationDataDTO
{
    public Guid FormFieldId { get; set; }
    public object? Value { get; set; }
}