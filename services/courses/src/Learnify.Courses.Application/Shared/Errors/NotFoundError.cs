using System.Net;

namespace Learnify.Courses.Application.Shared.Errors;

public sealed record NotFoundError : ErrorBase
{
    public NotFoundError(string error) : base(
        "Resource not found.",
        [error],
        HttpStatusCode.NotFound
    )
    {
    }
}