using System.Security.Cryptography;
using System.Text;
using EduSurveyAnalytics.Application.Interfaces.Services;

namespace EduSurveyAnalytics.Application.Services;

public class ShaHashingProvider : IHashingProvider
{
    public string Hash(string str)
    {
        var hashValue = SHA256.HashData(Encoding.UTF8.GetBytes(str));
        return BitConverter.ToString(hashValue);
    }
}