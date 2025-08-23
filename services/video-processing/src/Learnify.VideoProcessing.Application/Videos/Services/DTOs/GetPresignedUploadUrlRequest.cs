namespace Learnify.VideoProcessing.Application.Videos.Services.DTOs;

public sealed record GetPresignedUploadUrlRequest(
    string Name,
    TimeSpan? ExpirationTime = null
);