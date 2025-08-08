using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Categories.Errors;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;
using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

namespace Learnify.Courses.Application.Courses.UseCases.UpdateCourse;

public sealed class UpdateCourseUseCase(
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
) : IUpdateCourseUseCase
{
    public async Task<Result> ExecuteAsync(UpdateCourseRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.CourseId));

        if (course.IsInReview || course.IsDeleted)
            return Result.Fail(
                CoursesErrors.CourseCannotBeUpdated("Cannot update a course that is in review or deleted.")
            );

        var price = request.Price.HasValue
            ? Price.Create(request.Price.Value)
            : course.Price;

        var difficultyLevel = request.DifficultyLevel is null
            ? course.DifficultyLevel
            : Enum.Parse<DifficultyLevel>(request.DifficultyLevel);

        if (request.Categories is not null)
        {
            var allCategoriesExists = await categoryRepository
                .ExistsByIdsAsync(request.Categories, cancellationToken);

            if (!allCategoriesExists)
            {
                return Result.Fail(
                    CategoriesErrors.CategoryNotFound("One or more categories do not exist.")
                );
            }

            course.ClearCategories();
            request.Categories.ToList()
                .ForEach(categoryId => course.AddCategory(CategoryId.Create(categoryId)));
        }

        course.UpdateCourseInfo(
            request.Description,
            request.ImageUrl,
            price,
            request.Language,
            difficultyLevel
        );

        await courseRepository.UpdateAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}