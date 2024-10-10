namespace EduSurveyAnalytics.Application.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Change string's first letter to lower
    /// </summary>
    /// <param name="str">String to replace first letter</param>
    /// <returns>String str but with lowercased first letter</returns>
    public static string ToLowerFirstLetter(this string str)
        => char.ToLower(str[0]) + str[1..];
}