namespace EduSurveyAnalytics.Application.DTO;

public class FormPresentationDTO
{
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; }
    public string? OwnerPost { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public IEnumerable<FormFieldPresentationDTO> Fields { get; set; }
}