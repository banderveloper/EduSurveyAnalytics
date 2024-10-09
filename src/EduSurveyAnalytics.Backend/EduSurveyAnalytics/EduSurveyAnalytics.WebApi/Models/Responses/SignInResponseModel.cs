namespace EduSurveyAnalytics.WebApi.Models.Responses;

public class SignInResponseModel
{
    public string AccessToken { get; set; }
    public bool PasswordChangeRequired { get; set; }
}