using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.UpdateModule;

public sealed class UpdateModuleUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : IUpdateModuleUseCase
{
    public async Task<Result> ExecuteAsync(UpdateModuleRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.CourseId));

        if (course.IsInReview || course.IsDeleted)
            return Result.Fail(CoursesErrors.ModuleCannotBeAdded(""));

        course.UpdateModule(request.ModuleId, request.Title, request.Order);

        await courseRepository.UpdateAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}
