namespace EduSurveyAnalytics.WebApi.Models.Requests;

public class SignInRequestModel
{
    public string AccessCode { get; set; }
    public string? Password { get; set; }
}