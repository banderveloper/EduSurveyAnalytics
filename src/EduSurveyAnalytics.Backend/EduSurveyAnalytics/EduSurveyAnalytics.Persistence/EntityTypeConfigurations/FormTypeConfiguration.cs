using EduSurveyAnalytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSurveyAnalytics.Persistence.EntityTypeConfigurations;

public class FormTypeConfiguration : IEntityTypeConfiguration<Form>
{
    public void Configure(EntityTypeBuilder<Form> builder)
    {
        builder
            .HasOne(f => f.Owner)
            .WithMany(u => u.Forms)
            .HasForeignKey(f => f.OwnerId);

        builder
            .HasMany(f => f.Answers)
            .WithOne(a => a.Form)
            .HasForeignKey(f => f.FormId);

        builder
            .HasMany(f => f.FormFields)
            .WithOne(ff => ff.Form)
            .HasForeignKey(ff => ff.FormId);
    }
}