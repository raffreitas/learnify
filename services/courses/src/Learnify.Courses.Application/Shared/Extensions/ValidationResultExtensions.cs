using FluentValidation.Results;

using Learnify.Courses.Application.Shared.Errors;

namespace Learnify.Courses.Application.Shared.Extensions;

public static class ValidationResultExtensions
{
    public static ValidationError GetValidationError(this ValidationResult validationResult)
        => new([..validationResult.Errors.Select(x => x.ErrorMessage)]);
}