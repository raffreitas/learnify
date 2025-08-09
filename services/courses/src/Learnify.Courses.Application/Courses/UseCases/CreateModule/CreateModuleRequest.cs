using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.CreateModule;

public sealed record CreateModuleRequest
{
    public required Guid CourseId { get; init; }
    public required string Title { get; init; }
    public required int Order { get; init; }

    public ValidationResult Validate() => new CreateModuleRequestValidator().Validate(this);
}

internal sealed class CreateModuleRequestValidator : AbstractValidator<CreateModuleRequest>
{
    public CreateModuleRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();

        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(100);
        RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
    }
}