using FluentValidation;
using FluentValidation.Results;

using Learnify.Courses.Domain.Aggregates.Courses.Enums;

namespace Learnify.Courses.Application.Courses.UseCases.UpdateCourse;

public sealed record UpdateCourseRequest
{
    public Guid CourseId { get; init; }
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public decimal? Price { get; init; }
    public string? Language { get; init; }
    public string? DifficultyLevel { get; init; }
    public Guid[]? Categories { get; init; }

    public ValidationResult Validate() => new UpdateCourseRequestValidator().Validate(this);
};

internal sealed class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Description).MaximumLength(4000);
            RuleFor(x => x.Description).MinimumLength(20);
        });

        When(x => x.Price is not null, () =>
        {
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        });

        When(x => !string.IsNullOrEmpty(x.Language), () =>
        {
            RuleFor(x => x.Language).NotEmpty();
        });

        When(x => !string.IsNullOrEmpty(x.DifficultyLevel), () =>
        {
            RuleFor(x => x.DifficultyLevel).NotEmpty();
            RuleFor(x => x.DifficultyLevel).IsEnumName(typeof(DifficultyLevel));
        });

        When(x => !string.IsNullOrWhiteSpace(x.ImageUrl), () =>
        {
            RuleFor(x => x.ImageUrl).NotEmpty();
        });

        When(x => x.Categories is not null && x.Categories.Length > 0, () =>
        {
            RuleForEach(x => x.Categories).NotEmpty();
        });
    }
}