using System.Net;

namespace Learnify.Courses.Application.Shared.Errors;

public sealed record ConflictError : ErrorBase
{
    public ConflictError(string error) : base(
        "Resource already exists.",
        [error],
        HttpStatusCode.Conflict
    )
    {
    }
}