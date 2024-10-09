namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}