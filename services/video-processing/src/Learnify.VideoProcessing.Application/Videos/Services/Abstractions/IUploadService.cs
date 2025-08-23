using FluentResults;

using Learnify.VideoProcessing.Application.Videos.Services.DTOs;

namespace Learnify.VideoProcessing.Application.Videos.Services.Abstractions;

public interface IUploadService
{
    Task<Result<GetPresignedUploadUrlResponse>> GetPresignedUploadUrlAsync(
        GetPresignedUploadUrlRequest request,
        CancellationToken cancellationToken = default
    );
}