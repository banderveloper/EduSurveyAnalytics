namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IHashingProvider
{
    string Hash(string str);
}