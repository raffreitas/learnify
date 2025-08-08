using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.UpdateCourse;

public interface IUpdateCourseUseCase
{
    Task<Result> ExecuteAsync(UpdateCourseRequest request, CancellationToken cancellationToken = default);
}