using EduSurveyAnalytics.Application.Interfaces;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace EduSurveyAnalytics.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHashingProvider hashingProvider)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<FormField> FormFields { get; set; }
    public DbSet<FieldAnswer> FieldAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnswerTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FieldAnswerTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FormFieldTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FormTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
        
        // temp
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.NewGuid(),
            LastName = "Admin",
            FirstName = "Admin",
            MiddleName = "Admin",
            AccessCode = "admin",
            Post = "Admin",
            PasswordHash = hashingProvider.Hash("admin"),
            Permissions = new List<UserPermission> { UserPermission.All }
        });
    }
}