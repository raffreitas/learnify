using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.CreateCourse;

internal sealed class CreateCourseUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : ICreateCourseUseCase
{
    public async Task<Result<CreateCourseResponse>> ExecuteAsync(
        CreateCourseRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        if (await courseRepository.ExistsByTitleAsync(request.Title, cancellationToken))
            return Result.Fail(CoursesErrors.CourseAlreadyExists("Course with this title already exists."));

        Guid instructorId = new("018e3dd4-58aa-77e3-b663-8d14fcb672c1");
        var course = Course.CreateAsDraft(instructorId, request.Title);

        await courseRepository.AddAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateCourseResponse(course.Id);
    }
}