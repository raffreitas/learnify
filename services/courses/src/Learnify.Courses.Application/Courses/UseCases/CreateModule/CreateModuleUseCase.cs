using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.CreateModule;

public sealed class CreateModuleUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : ICreateModuleUseCase
{
    public async Task<Result<CreateModuleResponse>> ExecuteAsync(CreateModuleRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());
        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.CourseId));

        if (course.IsInReview || course.IsDeleted)
            return Result.Fail(CoursesErrors.ModuleCannotBeAdded(""));

        var module = Module.Create(course.Id, request.Title, request.Order);

        if (course.ModuleExists(module))
            return Result.Fail(CoursesErrors.ModuleAlreadyExists);

        course.AddModule(module);

        await courseRepository.UpdateAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return Result.Ok(new CreateModuleResponse(module.Id));
    }
}