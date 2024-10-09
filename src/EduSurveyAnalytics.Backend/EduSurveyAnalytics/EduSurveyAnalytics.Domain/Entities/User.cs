namespace EduSurveyAnalytics.Domain.Entities;

/// <summary>
/// User of service, some person from education system (student, teacher, director etc)
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Kind of login, student's document number, teacher contract number etc
    /// </summary>
    public string AccessCode { get; set; }

    /// <summary>
    /// Password, nullable because after creating new user during first sign-in he must enter first password
    /// </summary>
    public string? PasswordHash { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly? BirthDate { get; set; }

    /// <summary>
    /// Job title, profession (student, teacher, teacher position title etc)
    /// </summary>
    public string? Post { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
