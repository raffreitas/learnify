using FluentResults;

using Learnify.Courses.Application.Shared.Errors;

using Microsoft.AspNetCore.Mvc;

namespace Learnify.Courses.WebApi.Controllers;

[ApiController]
public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected ActionResult HandleProblem(ResultBase result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Result is successful, cannot handle problem.");

        if (result.Errors.All(e => e is ErrorBase) && result.Errors.Count == 1)
        {
            int statusCode = StatusCodes.Status400BadRequest;
            var error = result.Errors[0];
            error.Metadata.TryGetValue("StatusCode", out object? value);
            bool isValidStatusCodes = int.TryParse(value?.ToString(), out int errorStatusCode);

            if (isValidStatusCodes)
                statusCode = errorStatusCode;

            var problemDetails = new ProblemDetails
            {
                Title = error.Message,
                Status = statusCode,
                Extensions = { ["errors"] = error.Metadata.GetValueOrDefault("Errors") }
            };

            return StatusCode(statusCode, problemDetails);
        }

        var defaultProblemDetails = new ProblemDetails
        {
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Extensions = { ["Errors"] = Array.Empty<string>() }
        };

        return BadRequest(defaultProblemDetails);
    }
}