using Learnify.Courses.Application.Courses.UseCases.CreateCourse;
using Learnify.Courses.Application.Courses.UseCases.UpdateCourse;
using Learnify.Courses.WebApi.Models;

using Microsoft.AspNetCore.Mvc;

namespace Learnify.Courses.WebApi.Controllers;

[Route("api/courses/v1")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CoursesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CreateCourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCourseAsync(
        [FromBody] CreateCourseRequest request,
        [FromServices] ICreateCourseUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetCourseById), new { id = result.Value.CourseId }, result.Value)
            : HandleProblem(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateCourse(
        [FromRoute] Guid id,
        [FromBody] UpdateCourseModel model,
        [FromServices] IUpdateCourseUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(model.ToRequest(id), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleProblem(result);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetCourseById(Guid id)
    {
        return NotFound();
    }
}