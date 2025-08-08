using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.CreateCourse;

public sealed record CreateCourseRequest
{
    public required string Title { get; init; }
    public ValidationResult Validate() => new CreateCourseRequestValidator().Validate(this);
};

internal sealed class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(255);
        RuleFor(x => x.Title).MinimumLength(5);
    }
}