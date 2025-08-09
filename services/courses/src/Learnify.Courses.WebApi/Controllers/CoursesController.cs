using Learnify.Courses.Application.Courses.UseCases.CreateCourse;
using Learnify.Courses.Application.Courses.UseCases.CreateModule;
using Learnify.Courses.Application.Courses.UseCases.CreateLesson;
using Learnify.Courses.Application.Courses.UseCases.GetCourseById;
using Learnify.Courses.Application.Courses.UseCases.PublishCourse;
using Learnify.Courses.Application.Courses.UseCases.UpdateCourse;
using Learnify.Courses.Application.Courses.UseCases.UpdateLesson;
using Learnify.Courses.Application.Courses.UseCases.UpdateModule;
using Learnify.Courses.Application.Courses.UseCases.ReorderModules;
using Learnify.Courses.Application.Courses.UseCases.ReorderLessons;
using Learnify.Courses.Application.Courses.UseCases.RequestCourseReview;
using Learnify.Courses.Application.Courses.UseCases.UploadCourseImage;
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
    [ProducesResponseType<GetCourseByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseById(
        [FromRoute] Guid id,
        [FromServices] IGetCourseByIdUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(new GetCourseByIdRequest { Id = id }, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleProblem(result);
    }

    [HttpPatch("{id:guid}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PublishCourse(
        [FromRoute] Guid id,
        [FromServices] IPublishCourseUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(new PublishCourseRequest { CourseId = id }, cancellationToken);
        return result.IsSuccess ? NoContent() : HandleProblem(result);
    }

    [HttpPatch("{id:guid}/request-review")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RequestCourseReview(
        [FromRoute] Guid id,
        [FromServices] IRequestCourseReviewUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(new RequestCourseReviewRequest { CourseId = id }, cancellationToken);
        return result.IsSuccess ? NoContent() : HandleProblem(result);
    }

    [HttpPost("{id:guid}/modules")]
    [ProducesResponseType<CreateModuleResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateModuleAsync(
        [FromRoute] Guid id,
        [FromBody] CreateModuleModel model,
        [FromServices] ICreateModuleUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(model.ToRequest(id), cancellationToken);
        return result.IsSuccess
            ? Created("", new { id = result.Value.ModuleId })
            : HandleProblem(result);
    }

    [HttpPut("{id:guid}/modules/{moduleId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateModuleAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid moduleId,
        [FromBody] UpdateModuleModel model,
        [FromServices] IUpdateModuleUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(model.ToRequest(id, moduleId), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleProblem(result);
    }

    [HttpPost("{id:guid}/modules/{moduleId:guid}/lessons")]
    [ProducesResponseType<CreateLessonResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateLessonAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid moduleId,
        [FromBody] CreateLessonModel model,
        [FromServices] ICreateLessonUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(model.ToRequest(id, moduleId), cancellationToken);
        return result.IsSuccess
            ? Created("", new { id = result.Value.LessonId })
            : HandleProblem(result);
    }

    [HttpPut("{id:guid}/modules/{moduleId:guid}/lessons/{lessonId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateLessonAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid moduleId,
        [FromRoute] Guid lessonId,
        [FromBody] UpdateLessonModel model,
        [FromServices] IUpdateLessonUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(model.ToRequest(id, moduleId, lessonId), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleProblem(result);
    }

    [HttpPut("{id:guid}/modules:reorder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ReorderModulesAsync(
        [FromRoute] Guid id,
        [FromBody] ReorderModulesModel model,
        [FromServices] IReorderModulesUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(model.ToRequest(id), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleProblem(result);
    }

    [HttpPut("{id:guid}/modules/{moduleId:guid}/lessons:reorder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ReorderLessonsAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid moduleId,
        [FromBody] ReorderLessonsModel model,
        [FromServices] IReorderLessonsUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(model.ToRequest(id, moduleId), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleProblem(result);
    }

    [HttpPost("{id:guid}/upload-image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UploadImageAsync(
        [FromRoute] Guid id,
        [FromForm] IFormFile file,
        [FromServices] IUploadCourseImageUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        await using var stream = file.OpenReadStream();

        var request = new UploadCourseImageRequest
        {
            CourseId = id, FileStream = stream, ContentType = file.ContentType
        };
        var result = await useCase.ExecuteAsync(request, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : HandleProblem(result);
    }
}