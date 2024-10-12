using EduSurveyAnalytics.Application.DTO;

namespace EduSurveyAnalytics.WebApi.Models.Responses;

public class GetUserPresentationResponseModel
{
    public UserPresentationDTO? User { get; set; }
}