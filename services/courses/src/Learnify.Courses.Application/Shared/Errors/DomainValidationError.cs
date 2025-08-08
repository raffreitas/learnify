using System.Net;

namespace Learnify.Courses.Application.Shared.Errors;

public sealed record DomainValidationError : ErrorBase
{
    public DomainValidationError(string error) : base(
        "One or more domain validation errors occurred.",
        [error],
        HttpStatusCode.UnprocessableEntity
    )
    {
    }
}