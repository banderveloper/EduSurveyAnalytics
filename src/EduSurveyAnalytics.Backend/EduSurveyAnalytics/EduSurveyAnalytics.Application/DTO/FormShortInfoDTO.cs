namespace EduSurveyAnalytics.Application.DTO;

public class FormShortInfoDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string OwnerName { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}