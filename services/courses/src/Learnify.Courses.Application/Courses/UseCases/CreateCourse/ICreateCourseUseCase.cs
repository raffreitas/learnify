using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.CreateCourse;

public interface ICreateCourseUseCase
{
    Task<Result<CreateCourseResponse>> ExecuteAsync(
        CreateCourseRequest request,
        CancellationToken cancellationToken = default
    );
}