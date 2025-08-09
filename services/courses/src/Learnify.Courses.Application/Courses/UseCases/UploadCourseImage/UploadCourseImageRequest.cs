using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.UploadCourseImage;

public sealed record UploadCourseImageRequest
{
    public required Guid CourseId { get; init; }
    public required Stream FileStream { get; init; }
    public required string ContentType { get; init; }

    public ValidationResult Validate() => new UploadCourseImageRequestValidator().Validate(this);
}

public sealed class UploadCourseImageRequestValidator : AbstractValidator<UploadCourseImageRequest>
{
    public UploadCourseImageRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.FileStream).NotNull().WithMessage("File stream cannot be null.");
        RuleFor(x => x.ContentType)
            .NotEmpty()
            .Must(contentType => contentType.StartsWith("image/"))
            .WithMessage("Content type must be an image.");
    }
}