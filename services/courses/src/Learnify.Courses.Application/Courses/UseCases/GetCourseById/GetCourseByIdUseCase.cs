using FluentResults;

using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.GetCourseById;

internal sealed class GetCourseByIdUseCase(
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository
) : IGetCourseByIdUseCase
{
    public async Task<Result<GetCourseByIdResponse>> ExecuteAsync(
        GetCourseByIdRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.Id));

        var categories = await categoryRepository.GetByIdsAsync(
            [..course.Categories.Select(x => x.Value)],
            cancellationToken
        );

        return GetCourseByIdResponse.FromAggregates(course, [..categories]);
    }
}