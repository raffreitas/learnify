namespace Learnify.VideoProcessing.Application.Videos.Commands.CreateVideo;

public record CreateVideoCommandResult(
    Guid VideoId,
    string UploadUrl,
    TimeSpan Expiration
);