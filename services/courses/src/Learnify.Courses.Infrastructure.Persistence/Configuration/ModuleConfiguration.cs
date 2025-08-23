using Learnify.Courses.Domain.Aggregates.Courses.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnify.Courses.Infrastructure.Persistence.Configuration;

public sealed class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CourseId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => new { x.CourseId, x.Title })
            .IsUnique();

        builder.Property(x => x.Order)
            .IsRequired();

        builder.HasMany(x => x.Lessons)
            .WithOne()
            .HasForeignKey(x => x.ModuleId);
    }
}