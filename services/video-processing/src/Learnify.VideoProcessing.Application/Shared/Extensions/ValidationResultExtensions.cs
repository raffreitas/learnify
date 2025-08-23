using FluentValidation.Results;

using Learnify.VideoProcessing.Application.Shared.Errors;

namespace Learnify.VideoProcessing.Application.Shared.Extensions;

public static class ValidationResultExtensions
{
    public static ValidationError GetValidationError(this ValidationResult validationResult)
        => new([..validationResult.Errors.Select(x => x.ErrorMessage)]);
}