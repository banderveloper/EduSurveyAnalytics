namespace EduSurveyAnalytics.Application;

/// <summary>
/// Result pattern implementation, stores data as result of a function, or error code in case of error
/// </summary>
/// <typeparam name="TData">Result data type</typeparam>
public class Result<TData>
{
    public ErrorCode? ErrorCode { get; set; }
    public TData Data { get; set; }
    public bool Succeed => ErrorCode is null;

    public static Result<TData> Success(TData data = default) => new() { Data = data };
    public static Result<TData> Error(ErrorCode? errorCode) => new() { ErrorCode = errorCode };
}

// For results without data
public record struct None;