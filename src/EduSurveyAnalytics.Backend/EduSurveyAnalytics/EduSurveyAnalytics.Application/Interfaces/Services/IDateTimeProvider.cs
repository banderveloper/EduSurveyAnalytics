namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IDateTimeProvider
{
    /// <summary>
    /// NOW datetime provider, for mocking
    /// </summary>
    DateTimeOffset Now { get; }
}