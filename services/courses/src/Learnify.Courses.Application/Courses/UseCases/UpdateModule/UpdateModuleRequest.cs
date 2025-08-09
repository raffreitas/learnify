using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.UpdateModule;

public sealed record UpdateModuleRequest
{
    public required Guid CourseId { get; init; }
    public required Guid ModuleId { get; init; }
    public required string Title { get; init; }
    public required int Order { get; init; }

    public ValidationResult Validate() => new UpdateModuleRequestValidator().Validate(this);
}

internal sealed class UpdateModuleRequestValidator : AbstractValidator<UpdateModuleRequest>
{
    public UpdateModuleRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.ModuleId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
    }
}
