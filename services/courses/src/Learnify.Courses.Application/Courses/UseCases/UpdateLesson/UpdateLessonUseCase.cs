using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.UpdateLesson;

public sealed class UpdateLessonUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : IUpdateLessonUseCase
{
    public async Task<Result> ExecuteAsync(UpdateLessonRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.CourseId));

        if (course.IsInReview || course.IsDeleted)
            return Result.Fail(CoursesErrors.ModuleCannotBeAdded(""));

        var info = new LessonInfo(request.Title, request.Description, request.VideoUrl, request.Order, request.IsPublic);
        course.UpdateLesson(request.ModuleId, request.LessonId, info);

        await courseRepository.UpdateAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}
