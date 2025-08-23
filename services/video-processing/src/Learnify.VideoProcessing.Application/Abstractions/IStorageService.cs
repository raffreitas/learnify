namespace Learnify.VideoProcessing.Application.Abstractions;

public interface IStorageService
{
    Task<string> GetPresignedUploadUrlAsync(
        string name,
        TimeSpan? expirationTime = null,
        CancellationToken cancellationToken = default
    );
}