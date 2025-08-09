namespace Learnify.Courses.Application.Abstractions;

public interface IStorageService
{
    Task UploadFileAsync(
        Stream fileStream,
        string name,
        string contentType,
        CancellationToken cancellationToken = default
    );
}