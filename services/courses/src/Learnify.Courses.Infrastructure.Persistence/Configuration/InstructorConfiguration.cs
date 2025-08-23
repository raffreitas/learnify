using Learnify.Courses.Domain.Aggregates.Instructors;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnify.Courses.Infrastructure.Persistence.Configuration;

public sealed class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.ComplexProperty(x => x.Name, nameBuilder =>
        {
            nameBuilder.Property(x => x.FirstName)
                .HasMaxLength(50);
            nameBuilder.Property(x => x.LastName)
                .HasMaxLength(50);
        });

        builder.Property(x => x.Bio)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(200);
    }
}