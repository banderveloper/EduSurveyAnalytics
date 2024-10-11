using System.ComponentModel.DataAnnotations;

namespace EduSurveyAnalytics.WebApi.Models.Requests;

public class StopOtherSessionRequestModel
{
    [Required(ErrorMessage = "Fingerprint of other session is required")]
    public string Fingerprint { get; set; }
}