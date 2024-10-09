using EduSurveyAnalytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSurveyAnalytics.Persistence.EntityTypeConfigurations;

public class FieldAnswerTypeConfiguration : IEntityTypeConfiguration<FieldAnswer>
{
    public void Configure(EntityTypeBuilder<FieldAnswer> builder)
    {
        builder
            .HasOne(fa => fa.FormField)
            .WithMany(ff => ff.FieldAnswers)
            .HasForeignKey(fa => fa.FormFieldId);

        builder
            .HasOne(fa => fa.Answer)
            .WithMany(a => a.FieldAnswers)
            .HasForeignKey(fa => fa.AnswerId);
    }
}