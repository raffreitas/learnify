namespace Learnify.Courses.Application.Abstractions;

public interface IStorageService
{
    Task<string> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string bucketName,
        CancellationToken cancellationToken = default
    );
}