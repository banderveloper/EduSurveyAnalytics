namespace EduSurveyAnalytics.Domain.Entities;

/// <summary>
/// Survey form
/// </summary>
public class Form : BaseEntity
{
    /// <summary>
    /// Id of form's author
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Title of survey/form
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Time of form creation
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Time of last form update
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    // EF
    public User Owner { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public ICollection<FormField> FormFields { get; set; }
}
