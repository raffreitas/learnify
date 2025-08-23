using FluentValidation;
using FluentValidation.Results;

namespace Learnify.VideoProcessing.Application.Videos.Commands.CreateVideo;

public sealed record CreateVideoCommand
{
    public required string Filename { get; init; }
    public ValidationResult Validate() => new CreateVideoCommandValidator().Validate(this);
};

internal sealed class CreateVideoCommandValidator : AbstractValidator<CreateVideoCommand>
{
    public CreateVideoCommandValidator()
    {
        RuleFor(x => x.Filename).NotEmpty();
    }
}