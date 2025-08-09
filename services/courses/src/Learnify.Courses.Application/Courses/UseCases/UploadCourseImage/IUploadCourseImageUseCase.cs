using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.UploadCourseImage;

public interface IUploadCourseImageUseCase
{
    Task<Result<UploadCourseImageResponse>> ExecuteAsync(
        UploadCourseImageRequest request,
        CancellationToken cancellationToken = default
    );
}