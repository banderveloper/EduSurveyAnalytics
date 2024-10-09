using System.ComponentModel.DataAnnotations;

namespace EduSurveyAnalytics.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
