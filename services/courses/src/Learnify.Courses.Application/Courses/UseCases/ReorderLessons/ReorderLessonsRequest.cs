using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.ReorderLessons;

public sealed record ReorderLessonsRequest
{
    public required Guid CourseId { get; init; }
    public required Guid ModuleId { get; init; }
    public required Dictionary<Guid, int> Positions { get; init; }

    public ValidationResult Validate() => new ReorderLessonsRequestValidator().Validate(this);
}

internal sealed class ReorderLessonsRequestValidator : AbstractValidator<ReorderLessonsRequest>
{
    public ReorderLessonsRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.Positions).NotNull();
    RuleFor(x => x.Positions).Must(p => p.Count > 0).WithMessage("At least one position is required.");
    RuleForEach(x => x.Positions).Must(kv => kv.Value >= 0).WithMessage("Order must be non-negative.");
    }
}
