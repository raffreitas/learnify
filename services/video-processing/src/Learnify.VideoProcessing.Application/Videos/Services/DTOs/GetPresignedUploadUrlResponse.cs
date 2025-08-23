namespace Learnify.VideoProcessing.Application.Videos.Services.DTOs;

public sealed record GetPresignedUploadUrlResponse(
    string Url,
    TimeSpan Expiration
);