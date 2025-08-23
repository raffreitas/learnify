using Learnify.VideoProcessing.Domain.SeedWork;

namespace Learnify.VideoProcessing.Domain.Aggregates.Videos.Repositories;

public interface IVideoRepository : IRepository
{
    Task AddAsync(Video video, CancellationToken cancellationToken = default);
    Task<Video?> GetByFilenameAsync(string filename, CancellationToken cancellationToken = default);
}