using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.CreateLesson;

public sealed class CreateLessonUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : ICreateLessonUseCase
{
    public async Task<Result<CreateLessonResponse>> ExecuteAsync(CreateLessonRequest request, CancellationToken cancellationToken = default)
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
        course.AddLessonToModule(request.ModuleId, info);

        await courseRepository.UpdateAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        // find latest added lesson id from module
        var module = course.Modules.First(m => m.Id == request.ModuleId);
        var lessonId = module.Lessons.OrderByDescending(l => l.CreatedAt).First().Id;

        return Result.Ok(new CreateLessonResponse(lessonId));
    }
}
