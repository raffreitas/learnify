using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.SubmitCourseForReview;

public sealed record SubmitCourseForReviewRequest
{
    public Guid CourseId { get; init; }

    public ValidationResult Validate() => new SubmitCourseForReviewRequestValidator().Validate(this);
}

public sealed class SubmitCourseForReviewRequestValidator : AbstractValidator<SubmitCourseForReviewRequest>
{
    public SubmitCourseForReviewRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
    }
}