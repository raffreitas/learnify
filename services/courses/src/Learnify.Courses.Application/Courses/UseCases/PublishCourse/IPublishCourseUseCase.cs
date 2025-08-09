using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.PublishCourse;

public interface IPublishCourseUseCase
{
    Task<Result> ExecuteAsync(PublishCourseRequest request, CancellationToken cancellationToken = default);
}