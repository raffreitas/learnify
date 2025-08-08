using Learnify.Courses.Application.Categories.UseCases.CreateCategory;

using Microsoft.AspNetCore.Mvc;

namespace Learnify.Courses.WebApi.Controllers;

[Route("api/categories/v1")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CategoryController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CreateCategoryResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCategoryAsync(
        [FromBody] CreateCategoryRequest request,
        [FromServices] ICreateCategoryUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        var result = await useCase.ExecuteAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(string.Empty, new { id = result.Value.CategoryId }, result.Value)
            : HandleProblem(result);
    }
}