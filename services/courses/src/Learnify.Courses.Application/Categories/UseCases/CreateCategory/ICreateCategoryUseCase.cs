using FluentResults;

namespace Learnify.Courses.Application.Categories.UseCases.CreateCategory;

public interface ICreateCategoryUseCase
{
    Task<Result<CreateCategoryResponse>> ExecuteAsync(
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default
    );
}