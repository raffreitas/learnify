using Learnify.VideoProcessing.Domain.Aggregates.Videos;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnify.VideoProcessing.Infrastructure.Persistence.Configurations;

internal sealed class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .ValueGeneratedOnAdd();

        builder.Property(v => v.Filename)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => x.Filename)
            .IsUnique();

        builder.Property(v => v.Url)
            .HasMaxLength(2048);

        builder.Property(v => v.Duration)
            .HasConversion(
                d => d.HasValue ? d.Value.TotalSeconds : default(double?),
                s => s.HasValue ? TimeSpan.FromSeconds(s.Value) : null
            );

        builder.Property(v => v.CreatedAt)
            .IsRequired();

        builder.Property(v => v.UpdatedAt)
            .IsRequired();
    }
}