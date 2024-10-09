using EduSurveyAnalytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduSurveyAnalytics.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Form> Forms { get; set; }
    DbSet<Answer> Answers { get; set; }
    DbSet<FormField> FormFields { get; set; }
    DbSet<FieldAnswer> FieldAnswers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}