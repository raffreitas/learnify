using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.ReorderModules;

public sealed record ReorderModulesRequest
{
    public required Guid CourseId { get; init; }
    public required Dictionary<Guid, int> Positions { get; init; }

    public ValidationResult Validate() => new ReorderModulesRequestValidator().Validate(this);
}

internal sealed class ReorderModulesRequestValidator : AbstractValidator<ReorderModulesRequest>
{
    public ReorderModulesRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
    RuleFor(x => x.Positions).NotNull();
    RuleFor(x => x.Positions).Must(p => p.Count > 0).WithMessage("At least one position is required.");
    RuleForEach(x => x.Positions).Must(kv => kv.Value >= 0).WithMessage("Order must be non-negative.");
    }
}
