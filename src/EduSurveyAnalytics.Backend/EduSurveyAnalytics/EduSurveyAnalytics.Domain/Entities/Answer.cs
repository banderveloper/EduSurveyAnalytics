namespace EduSurveyAnalytics.Domain.Entities;

/// <summary>
/// User's form answer, has child elements for each input
/// </summary>
public class Answer : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid FormId { get; set; }

    // EF
    public User User { get; set; }
    public Form Form { get; set; }
    public ICollection<FieldAnswer> FieldAnswers { get; set; }
}
