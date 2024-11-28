using EduSurveyAnalytics.Application.DTO;

namespace EduSurveyAnalytics.WebApi.Models.Responses;

public class GetFormsShortsResponseModel
{
    public IEnumerable<FormShortInfoDTO> Forms { get; set; }
}