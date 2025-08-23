using System.Net;

using FluentResults;

namespace Learnify.VideoProcessing.Application.Shared.Errors;

public abstract record ErrorBase : IError
{
    public string Message { get; }
    public Dictionary<string, object> Metadata { get; }
    public List<IError> Reasons { get; } = [];

    protected ErrorBase(string message, string[] errors, HttpStatusCode statusCode)
    {
        Message = message;
        Metadata = new Dictionary<string, object>() { { "Errors", errors }, { "StatusCode", (int)statusCode } };
    }
}