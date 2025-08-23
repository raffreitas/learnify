using System.Net;

namespace Learnify.VideoProcessing.Application.Shared.Errors;

public sealed record ValidationError : ErrorBase
{
    public ValidationError(string[] errors) : base(
        "One or more validation errors occurred.",
        errors,
        HttpStatusCode.BadRequest
    )
    {
    }
}