namespace EduSurveyAnalytics.WebApi.Models.Requests;

public class CreateUserRequestModel
{
    public string AccessCode { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Post { get; set; }
}