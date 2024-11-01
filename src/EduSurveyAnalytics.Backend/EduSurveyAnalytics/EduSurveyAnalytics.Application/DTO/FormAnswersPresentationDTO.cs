namespace EduSurveyAnalytics.Application.DTO;

public class FormAnswersPresentationDTO
{
    public string FormTitle { get; set; }
    public IEnumerable<FormFieldAnswersPresentationDTO> FormFields { get; set; }
}

public class FormFieldAnswersPresentationDTO
{
    public string Title { get; set; }
    public IEnumerable<FormFieldAnswerPresentationDTO> Answers { get; set; }
}

public class FormFieldAnswerPresentationDTO
{
    public string UserName { get; set; }
    public object? Value{ get; set; }
    public DateTimeOffset AnsweredAt { get; set; }
}