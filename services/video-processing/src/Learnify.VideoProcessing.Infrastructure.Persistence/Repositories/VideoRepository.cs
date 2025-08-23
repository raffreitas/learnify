using Learnify.VideoProcessing.Domain.Aggregates.Videos;
using Learnify.VideoProcessing.Domain.Aggregates.Videos.Repositories;
using Learnify.VideoProcessing.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

namespace Learnify.VideoProcessing.Infrastructure.Persistence.Repositories;

internal sealed class VideoRepository(ApplicationDbContext dbContext) : IVideoRepository
{
    public async Task AddAsync(Video video, CancellationToken cancellationToken = default)
        => await dbContext.Videos.AddAsync(video, cancellationToken);

    public async Task<Video?> GetByFilenameAsync(string filename, CancellationToken cancellationToken = default)
        => await dbContext.Videos.Where(v => v.Filename == filename).FirstOrDefaultAsync(cancellationToken);
}