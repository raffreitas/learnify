using FluentValidation;
using FluentValidation.Results;

namespace Learnify.Courses.Application.Courses.UseCases.GetCourseById;

public sealed record GetCourseByIdRequest
{
    public required Guid Id { get; init; }

    public ValidationResult Validate() => new GetCourseByIdRequestValidator().Validate(this);
};

internal sealed class GetCourseByIdRequestValidator : AbstractValidator<GetCourseByIdRequest>
{
    public GetCourseByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}