using System.Text.Json.Serialization;
using EduSurveyAnalytics.Application.Converters;

namespace EduSurveyAnalytics.Application;

// Result error code for responses
[JsonConverter(typeof(SnakeCaseStringEnumConverter<ErrorCode>))]
public enum ErrorCode
{
    Unknown,
    InvalidModel, // 422
    
    InvalidCredentials, // auth error
    
    AccessCodeAlreadyExists,
    UserNotFound
}