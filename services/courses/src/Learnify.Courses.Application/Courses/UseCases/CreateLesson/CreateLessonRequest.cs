using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.CreateLesson;

public sealed record CreateLessonRequest
{
    public required Guid CourseId { get; init; }
    public required Guid ModuleId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string VideoUrl { get; init; }
    public required int Order { get; init; }
    public required bool IsPublic { get; init; }

    public ValidationResult Validate() => new CreateLessonRequestValidator().Validate(this);
}

internal sealed class CreateLessonRequestValidator : AbstractValidator<CreateLessonRequest>
{
    public CreateLessonRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.VideoUrl).NotEmpty().MaximumLength(2048);
        RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
    }
}
