using System.Text.Json.Serialization;
using EduSurveyAnalytics.Domain.Enums;

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

    /// <summary>
    /// List of constraints for frontend
    /// </summary>
    public ICollection<FormFieldConstraint> Constraints { get; set; } =
        new List<FormFieldConstraint>();

    // EF
    [JsonIgnore] public Form Form { get; set; }
    [JsonIgnore] public ICollection<FieldAnswer> FieldAnswers { get; set; }
}