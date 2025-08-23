using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

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

        builder.ComplexProperty(x => x.Slug, slugBuilder =>
        {
            slugBuilder.Property(x => x.Value)
                .HasColumnName("slug")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.ComplexProperty(x => x.Media, mediaBuilder =>
        {
            mediaBuilder.Property(x => x.AssetId)
                .HasConversion(x => x.Value, x => MediaAssetId.Create(x))
                .IsRequired();

            mediaBuilder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            mediaBuilder.Property(x => x.Duration)
                .IsRequired(false);

            mediaBuilder.Property(x => x.FailureReason)
                .IsRequired(false);
        });
    }
}