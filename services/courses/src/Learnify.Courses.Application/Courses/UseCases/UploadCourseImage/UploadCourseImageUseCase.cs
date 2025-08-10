using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Storage;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.UploadCourseImage;

internal sealed class UploadCourseImageUseCase(
    IStorageService storageService,
    ICourseRepository courseRepository
) : IUploadCourseImageUseCase
{
    public async Task<Result<UploadCourseImageResponse>> ExecuteAsync(
        UploadCourseImageRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.ExistsByIdAsync(request.CourseId, cancellationToken);
        if (!course)
            return Result.Fail(CoursesErrors.CourseNotFound(request.CourseId));

        var streamValidationResult = request.FileStream.Length switch
        {
            0 => Result.Fail(CoursesErrors.ImageFileEmpty),
            > 5 * 1024 * 1024 => Result.Fail(CoursesErrors.ImageFileTooLarge),
            _ => Result.Ok()
        };

        if (streamValidationResult.IsFailed)
            return streamValidationResult;

        var fileExtension = ExtractExtension(request.ContentType);
        if (fileExtension.IsFailed)
            return Result.Fail(CoursesErrors.InvalidImageContentType);

        var fileKey = $"courses/assets/{request.CourseId}/cover/original{fileExtension.Value}";

        await storageService.UploadFileAsync(
            request.FileStream,
            fileKey,
            request.ContentType,
            cancellationToken
        );

        return new UploadCourseImageResponse(fileKey);
    }

    private static Result<string> ExtractExtension(string contentType) => contentType switch
    {
        "image/jpeg" => ".jpg",
        "image/png" => ".png",
        _ => Result.Fail("Unsupported image content type")
    };
}