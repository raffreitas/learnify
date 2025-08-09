using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Categories.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Categories;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;

namespace Learnify.Courses.Application.Categories.UseCases.CreateCategory;

public sealed class CreateCategoryUseCase(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
) : ICreateCategoryUseCase
{
    public async Task<Result<CreateCategoryResponse>> ExecuteAsync(
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        if (await categoryRepository.ExistsByNameAsync(request.Name, cancellationToken))
            return Result.Fail(CategoriesErrors.CategoryAlreadyExists);

        var category = Category.Create(request.Name);

        await categoryRepository.AddAsync(category, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateCategoryResponse(category.Id);
    }
}