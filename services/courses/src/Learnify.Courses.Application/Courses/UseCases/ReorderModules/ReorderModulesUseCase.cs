using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.ReorderModules;

public sealed class ReorderModulesUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : IReorderModulesUseCase
{
    public async Task<Result> ExecuteAsync(ReorderModulesRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.CourseId));

        if (course.IsInReview || course.IsDeleted)
            return Result.Fail(CoursesErrors.ModuleCannotBeAdded(""));

        // Validate membership: positions must include exactly all modules of the course
        var moduleIds = course.Modules.Select(m => m.Id).ToHashSet();
        if (request.Positions.Count != moduleIds.Count)
            return Result.Fail(CoursesErrors.InvalidReorderPayload("Positions must include all modules exactly once."));

        var missing = request.Positions.Keys.FirstOrDefault(id => !moduleIds.Contains(id));
        if (missing != Guid.Empty)
            return Result.Fail(CoursesErrors.ModuleNotFound(missing));

        // Validate orders: non-negative, unique, and contiguous from 0..N-1
        var orders = request.Positions.Values.ToList();
        if (orders.Any(o => o < 0))
            return Result.Fail(CoursesErrors.InvalidReorderPayload("Order values must be non-negative."));
        if (orders.Distinct().Count() != orders.Count)
            return Result.Fail(CoursesErrors.InvalidReorderPayload("Order values must be unique."));
        var n = orders.Count;
        if (orders.Min() != 0 || orders.Max() != n - 1)
            return Result.Fail(CoursesErrors.InvalidReorderPayload("Order values must be contiguous from 0 to N-1."));

        course.ReorderModules(request.Positions);

        await courseRepository.UpdateAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}
