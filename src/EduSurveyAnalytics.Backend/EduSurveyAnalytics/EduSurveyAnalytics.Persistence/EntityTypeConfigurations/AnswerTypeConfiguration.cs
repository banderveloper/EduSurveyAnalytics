using EduSurveyAnalytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSurveyAnalytics.Persistence.EntityTypeConfigurations;

public class AnswerTypeConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder
            .HasOne(a => a.User)
            .WithMany(u => u.Answers)
            .HasForeignKey(a => a.UserId);

        builder
            .HasOne(a => a.Form)
            .WithMany(f => f.Answers)
            .HasForeignKey(a => a.FormId);

        builder
            .HasMany(a => a.FieldAnswers)
            .WithOne(fa => fa.Answer)
            .HasForeignKey(fa => fa.AnswerId);
    }
}