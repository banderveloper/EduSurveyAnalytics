namespace EduSurveyAnalytics.Domain.Entities;

/// <summary>
/// One input inside a form
/// </summary>
public class FormField : BaseEntity
{
    /// <summary>
    /// Id of parent form where field is placed
    /// </summary>
    public Guid FormId { get; set; }

    /// <summary>
    /// Title of input which is shown above the input
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Placing order index
    /// </summary>
    public int Order { get; set; }
}
