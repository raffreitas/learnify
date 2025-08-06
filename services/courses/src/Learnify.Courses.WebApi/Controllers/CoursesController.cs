using Learnify.Courses.Application.Courses.UseCases.CreateCourse;

using Microsoft.AspNetCore.Mvc;

namespace Learnify.Courses.WebApi.Controllers;

[ApiController]
[Route("api/courses/v1")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CoursesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CreateCourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateCourseAsync(
        [FromBody] CreateCourseRequest request,
        [FromServices] ICreateCourseUseCase useCase,
        CancellationToken cancellationToken)
    {
        var response = await useCase.ExecuteAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetCourseById), new { id = response.CourseId }, response);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetCourseById(Guid id)
    {
        return NotFound();
    }
}