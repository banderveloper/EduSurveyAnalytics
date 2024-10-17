using EduSurveyAnalytics.Domain.Enums;

namespace EduSurveyAnalytics.WebApi.Models.Requests;

public class CreateFormRequestModel
{
    public string FormTitle { get; set; }
    public IEnumerable<CreateFormFieldRequestModel> FormFields { get; set; }
}

public class CreateFormFieldRequestModel
{
    public string Title { get; set; }
    public int Order { get; set; }
    public ICollection<FormFieldConstraint> Constraints { get; set; }
}