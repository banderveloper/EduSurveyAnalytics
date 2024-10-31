using System.Text.Json;
using EduSurveyAnalytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EduSurveyAnalytics.Persistence.EntityTypeConfigurations;

public class FieldAnswerTypeConfiguration : IEntityTypeConfiguration<FieldAnswer>
{
    public void Configure(EntityTypeBuilder<FieldAnswer> builder)
    {
        var converter = new ValueConverter<object?, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), // Serialize object to JSON
            v => JsonSerializer.Deserialize<object>(v, (JsonSerializerOptions)null) // Deserialize JSON to object
        );

        builder
            .Property(fa => fa.Value)
            .HasColumnType("jsonb") // JSONB column type in PostgreSQL
            .HasConversion(converter); // Use converter for serialization
        
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