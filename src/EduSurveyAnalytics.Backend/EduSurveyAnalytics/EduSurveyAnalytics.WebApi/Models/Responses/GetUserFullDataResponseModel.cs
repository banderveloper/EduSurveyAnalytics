using EduSurveyAnalytics.Application.DTO;

namespace EduSurveyAnalytics.WebApi.Models.Responses;

public class GetUserFullDataResponseModel
{
    public UserFullDataDTO? User { get; set; }
}