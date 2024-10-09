using EduSurveyAnalytics.Domain.Entities;
using EduSurveyAnalytics.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSurveyAnalytics.Persistence.EntityTypeConfigurations;

public class FormFieldTypeConfiguration : IEntityTypeConfiguration<FormField>
{
    public void Configure(EntityTypeBuilder<FormField> builder)
    {
        builder
            .Property(ff => ff.Constraints)
            .HasConversion(
                arr => arr.Select(ffc => (int)ffc).ToArray(),
                arr => arr.Select(ffc => (FormFieldConstraint)ffc).ToList()
            )
            .HasColumnType("integer[]");

        builder
            .HasOne(ff => ff.Form)
            .WithMany(f => f.FormFields)
            .HasForeignKey(ff => ff.FormId);

        builder
            .HasMany(ff => ff.FieldAnswers)
            .WithOne(fa => fa.FormField)
            .HasForeignKey(fa => fa.FormFieldId);
    }
}