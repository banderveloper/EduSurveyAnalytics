namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IHashingProvider
{
    /// <summary>
    /// Hash string using some hashing algorythm
    /// </summary>
    /// <param name="str">String to hash</param>
    /// <returns>Hashed string</returns>
    string Hash(string str);
}