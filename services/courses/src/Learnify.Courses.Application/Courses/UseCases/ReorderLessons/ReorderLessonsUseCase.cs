using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.ReorderLessons;

public sealed class ReorderLessonsUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : IReorderLessonsUseCase
{
    public async Task<Result> ExecuteAsync(ReorderLessonsRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.CourseId));

        if (course.IsInReview || course.IsDeleted)
            return Result.Fail(CoursesErrors.ModuleCannotBeAdded(""));

        var module = course.Modules.FirstOrDefault(m => m.Id == request.ModuleId);
        if (module is null)
            return Result.Fail(CoursesErrors.ModuleNotFound(request.ModuleId));

        // Validate membership: positions must include exactly all lessons of the module (or we can allow partial?).
        var lessonIds = module.Lessons.Select(l => l.Id).ToHashSet();
        if (request.Positions.Count != lessonIds.Count)
            return Result.Fail(CoursesErrors.InvalidReorderPayload("Positions must include all lessons exactly once."));

        var missing = request.Positions.Keys.FirstOrDefault(id => !lessonIds.Contains(id));
        if (missing != Guid.Empty)
            return Result.Fail(CoursesErrors.LessonNotFound(missing));

        // Validate orders
        var orders = request.Positions.Values.ToList();
        if (orders.Any(o => o < 0))
            return Result.Fail(CoursesErrors.InvalidReorderPayload("Order values must be non-negative."));
        if (orders.Distinct().Count() != orders.Count)
            return Result.Fail(CoursesErrors.InvalidReorderPayload("Order values must be unique."));
        var n = orders.Count;
        if (orders.Min() != 0 || orders.Max() != n - 1)
            return Result.Fail(CoursesErrors.InvalidReorderPayload("Order values must be contiguous from 0 to N-1."));

        course.ReorderLessons(request.ModuleId, request.Positions);

        await courseRepository.UpdateAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}
