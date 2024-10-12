using EduSurveyAnalytics.Domain.Enums;

namespace EduSurveyAnalytics.Application.DTO;

public class UserFullDataDTO
{
    public Guid UserId { get; set; }
    
    public string AccessCode { get; set; }
    
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    
    public DateOnly? BirthDate { get; set; }
    public string? Post { get; set; }
    
    public IEnumerable<UserPermission> Permissions { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}