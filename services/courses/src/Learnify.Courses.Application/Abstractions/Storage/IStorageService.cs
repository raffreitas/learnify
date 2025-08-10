namespace Learnify.Courses.Application.Abstractions.Storage;

public interface IStorageService
{
    Task UploadFileAsync(
        Stream fileStream,
        string name,
        string contentType,
        CancellationToken cancellationToken = default
    );

    Task<string> GetFileUrlAsync(
        string name,
        TimeSpan? expirationTime = null,
        CancellationToken cancellationToken = default
    );
}