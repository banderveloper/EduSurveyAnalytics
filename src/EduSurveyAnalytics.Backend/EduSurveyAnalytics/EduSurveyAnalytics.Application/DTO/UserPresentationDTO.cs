namespace EduSurveyAnalytics.Application.DTO;

public class UserPresentationDTO
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Post { get; set; }
}