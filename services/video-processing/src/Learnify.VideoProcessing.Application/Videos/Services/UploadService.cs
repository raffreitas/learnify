using FluentResults;

using Learnify.VideoProcessing.Application.Abstractions;
using Learnify.VideoProcessing.Application.Videos.Services.Abstractions;
using Learnify.VideoProcessing.Application.Videos.Services.DTOs;

namespace Learnify.VideoProcessing.Application.Videos.Services;

internal sealed class UploadService(IStorageService storageService) : IUploadService
{
    public async Task<Result<GetPresignedUploadUrlResponse>> GetPresignedUploadUrlAsync(
        GetPresignedUploadUrlRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var expirationTime = request.ExpirationTime ?? TimeSpan.FromMinutes(15);
        if (expirationTime <= TimeSpan.Zero || expirationTime > TimeSpan.FromHours(12))
        {
            return Result.Fail(new Error("Expiration time must be between 1 second and 12 hours."));
        }

        var url = await storageService.GetPresignedUploadUrlAsync(
            request.Name,
            request.ExpirationTime,
            cancellationToken
        );

        return new GetPresignedUploadUrlResponse(
            url, request.ExpirationTime ?? TimeSpan.FromMinutes(15)
        );
    }
}