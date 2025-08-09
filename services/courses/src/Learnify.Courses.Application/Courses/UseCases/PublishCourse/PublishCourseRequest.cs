using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.PublishCourse;

public sealed record PublishCourseRequest
{
    public Guid CourseId { get; init; }

    public ValidationResult Validate() => new SubmitReviewRequestValidator().Validate(this);
}

public sealed class SubmitReviewRequestValidator : AbstractValidator<PublishCourseRequest>
{
    public SubmitReviewRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
    }
}