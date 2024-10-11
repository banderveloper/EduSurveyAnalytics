using EduSurveyAnalytics.Application.DTO;

namespace EduSurveyAnalytics.WebApi.Models.Responses;

public class GetOtherRefreshSessionsResponseModel
{
    public IEnumerable<RefreshSessionPresentationDTO> Sessions { get; set; }
}
