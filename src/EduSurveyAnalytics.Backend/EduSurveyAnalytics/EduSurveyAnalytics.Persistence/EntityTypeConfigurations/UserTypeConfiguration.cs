using EduSurveyAnalytics.Domain.Entities;
using EduSurveyAnalytics.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSurveyAnalytics.Persistence.EntityTypeConfigurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(u => u.AccessCode).IsUnique();

        builder
            .Property(u => u.Permissions)
            .HasConversion(
                arr => arr.Select(perm => (int)perm).ToArray(),
                arr => arr.Select(perm => (UserPermission)perm).ToList()
            )
            .HasColumnType("integer[]");

        builder.HasMany(u => u.Answers)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        builder.HasMany(u => u.Forms)
            .WithOne(f => f.Owner)
            .HasForeignKey(f => f.OwnerId);
    }
}