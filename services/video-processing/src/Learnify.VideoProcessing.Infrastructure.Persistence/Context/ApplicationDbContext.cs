using Learnify.VideoProcessing.Domain.Aggregates.Videos;
using Learnify.VideoProcessing.Domain.SeedWork;
using Learnify.VideoProcessing.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;

namespace Learnify.VideoProcessing.Infrastructure.Persistence.Context;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Video> Videos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfiguration(new VideoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}