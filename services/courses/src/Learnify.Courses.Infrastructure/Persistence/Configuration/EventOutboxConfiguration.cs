using Learnify.Courses.Infrastructure.Persistence.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnify.Courses.Infrastructure.Persistence.Configuration;

internal sealed class EventOutboxConfiguration : IEntityTypeConfiguration<EventOutbox>
{
    public void Configure(EntityTypeBuilder<EventOutbox> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Content).IsRequired().HasColumnType("jsonb");
        builder.Property(x => x.OccurredAt).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.ProcessedAt);

        builder.HasIndex(x => x.OccurredAt).IsDescending();
    }
}