using EduSurveyAnalytics.Application.Interfaces.Services;

namespace EduSurveyAnalytics.Application.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}