using Learnify.Courses.Application.Abstractions.VideoProcessing.DTOs;

namespace Learnify.Courses.Application.Abstractions.VideoProcessing;

public interface IVideoProcessingService
{
    Task<string> GetPresignedUploadUrlAsync(
        string name,
        TimeSpan? expirationTime = null,
        CancellationToken cancellationToken = default
    );

    Task<CreateVideoResponseDto> CreateVideoAsync(
        string filename,
        CancellationToken cancellationToken = default
    );
}