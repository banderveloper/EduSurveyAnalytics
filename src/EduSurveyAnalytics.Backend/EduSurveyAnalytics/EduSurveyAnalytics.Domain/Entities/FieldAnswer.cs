using System.Text.Json.Serialization;

namespace EduSurveyAnalytics.Domain.Entities;

/// <summary>
/// Part of answer, one filled field
/// </summary>
public class FieldAnswer : BaseEntity
{
    public Guid FormFieldId { get; set; }
    public Guid AnswerId { get; set; }

    /// <summary>
    /// Input value
    /// </summary>
    public object? Value { get; set; }

    // EF
    [JsonIgnore] public FormField FormField { get; set; }
    [JsonIgnore] public Answer Answer { get; set; }
}