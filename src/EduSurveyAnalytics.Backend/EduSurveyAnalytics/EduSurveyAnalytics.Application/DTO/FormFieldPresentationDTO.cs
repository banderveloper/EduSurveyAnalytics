using EduSurveyAnalytics.Domain.Enums;

namespace EduSurveyAnalytics.Application.DTO;

public class FormFieldPresentationDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public ICollection<FormFieldConstraint> Constraints { get; set; }
}