namespace EduSurveyAnalytics.WebApi.Models.Requests;

public class CreateAnswerRequestModel
{
    public Guid FormId { get; set; }
    public IEnumerable<FieldAnswerRequestModel> FieldAnswers { get; set; }
}

public class FieldAnswerRequestModel
{
    public Guid FormFieldId { get; set; }
    public object? Value { get; set; }
}