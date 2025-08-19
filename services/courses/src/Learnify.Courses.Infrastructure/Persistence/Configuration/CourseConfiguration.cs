using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.Domain.Aggregates.Instructors;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnify.Courses.Infrastructure.Persistence.Configuration;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Instructor)
            .HasColumnName("instructor_id")
            .HasConversion(x => x.Value, x => InstructorId.Create(x))
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.Title)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.ComplexProperty(x => x.Price, priceBuilder =>
        {
            priceBuilder.Property(x => x.Currency)
                .HasColumnName("price_currency")
                .HasMaxLength(3)
                .IsRequired();

            priceBuilder.Property(x => x.Value)
                .HasColumnName("price_value")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.Property(x => x.Language)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(x => x.DifficultyLevel)
            .HasConversion<string>()
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.HasMany(x => x.Modules)
            .WithOne()
            .HasForeignKey(x => x.CourseId);

        builder.OwnsMany(x => x.Categories, categoryBuilder =>
        {
            categoryBuilder.ToTable("course_categories");
            categoryBuilder.WithOwner().HasForeignKey("course_id");
            categoryBuilder.Property(x => x.Value)
                .HasColumnName("category_id")
                .IsRequired();
        });

        builder.Property(x => x.IsRevised)
            .IsRequired()
            .HasDefaultValue(false);
    }
}