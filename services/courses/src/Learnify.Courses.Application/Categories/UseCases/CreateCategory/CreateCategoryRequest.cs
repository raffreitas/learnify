using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Categories.UseCases.CreateCategory;

public sealed record CreateCategoryRequest
{
    public required string Name { get; init; }

    public ValidationResult Validate() => new CreateCategoryRequestValidator().Validate(this);
};

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(100);
    }
}