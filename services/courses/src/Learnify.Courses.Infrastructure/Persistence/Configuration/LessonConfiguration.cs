using Learnify.Courses.Domain.Aggregates.Courses.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnify.Courses.Infrastructure.Persistence.Configuration;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ModuleId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => new { x.ModuleId, x.Title })
            .IsUnique();

        builder.Property(x => x.Order)
            .IsRequired();

        builder.Property(x => x.IsPublic)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.VideoUrl)
            .HasMaxLength(500);
    }
}