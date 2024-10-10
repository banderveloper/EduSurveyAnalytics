using System.ComponentModel.DataAnnotations;

namespace EduSurveyAnalytics.WebApi.Models.Requests;

public class SignInRequestModel
{
    [Required(ErrorMessage = "Access code is required")]
    public string AccessCode { get; set; }
    public string? Password { get; set; }
    
    [Required(ErrorMessage = "Fingerprint is required")]
    public string Fingerprint { get; set; }
}