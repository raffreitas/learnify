using Learnify.Courses.Application.Abstractions.VideoProcessing;
using Learnify.Courses.Application.Abstractions.VideoProcessing.DTOs;

namespace Learnify.Courses.Infrastructure.Integrations.VideoProcessing;

internal sealed class VideoProcessingService(
    VideoService.VideoServiceClient videoServiceClient,
    UploadManager.UploadManagerClient uploadManagerClient
) : IVideoProcessingService
{
    public Task<string> GetPresignedUploadUrlAsync(
        string name,
        TimeSpan? expirationTime = null,
        CancellationToken cancellationToken = default
    )
    {
        var expiration = expirationTime ?? TimeSpan.FromMinutes(15);
        var request = new GetPresignedURLRequest { FileName = name, Expiration = (int)expiration.TotalSeconds };
        var grpcResponse = uploadManagerClient.GetPresignedURL(request, cancellationToken: cancellationToken);
        return Task.FromResult(grpcResponse.Url);
    }

    public Task<CreateVideoResponseDto> CreateVideoAsync(string filename,
        CancellationToken cancellationToken = default)
    {
        var request = new CreateVideoRequest { FileName = filename };
        var grpcResponse = videoServiceClient.CreateVideo(request, cancellationToken: cancellationToken);

        var response = new CreateVideoResponseDto(
            grpcResponse.VideoId,
            grpcResponse.UploadUrl,
            grpcResponse.UploadExpiration
        );

        return Task.FromResult(response);
    }
}