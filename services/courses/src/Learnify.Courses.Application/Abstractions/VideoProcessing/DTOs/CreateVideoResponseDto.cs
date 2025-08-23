namespace Learnify.Courses.Application.Abstractions.VideoProcessing.DTOs;

public record CreateVideoResponseDto(string VideoId, string UploadUrl, int UploadExpiration);