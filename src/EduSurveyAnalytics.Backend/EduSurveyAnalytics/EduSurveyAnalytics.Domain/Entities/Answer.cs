using System.Text.Json.Serialization;

namespace EduSurveyAnalytics.Domain.Entities;

/// <summary>
/// User's form answer, has child elements for each input
/// </summary>
public class Answer : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid FormId { get; set; }

    // EF

    [JsonIgnore] public User User { get; set; }
    [JsonIgnore] public Form Form { get; set; }
    [JsonIgnore] public ICollection<FieldAnswer> FieldAnswers { get; set; }
}