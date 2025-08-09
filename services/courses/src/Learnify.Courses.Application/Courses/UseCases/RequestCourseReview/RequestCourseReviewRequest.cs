using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.RequestCourseReview;

public sealed record RequestCourseReviewRequest
{
    public Guid CourseId { get; init; }

    public ValidationResult Validate() => new RequestCourseReviewRequestValidator().Validate(this);
}

public sealed class RequestCourseReviewRequestValidator : AbstractValidator<RequestCourseReviewRequest>
{
    public RequestCourseReviewRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
    }
}