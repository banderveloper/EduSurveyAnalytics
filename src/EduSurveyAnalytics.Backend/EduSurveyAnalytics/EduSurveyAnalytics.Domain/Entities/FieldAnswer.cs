namespace EduSurveyAnalytics.Domain.Entities;

/// <summary>
/// Part of answer, one filled field
/// </summary>
public class FieldAnswer : BaseEntity
{
    public Guid FormFieldId { get; set; }
    public Guid AnswerId { get; set; }

    /// <summary>
    /// Input text
    /// </summary>
    public string? Value { get; set; }

    // EF
    public FormField FormField { get; set; }
    public Answer Answer { get; set; }
}
