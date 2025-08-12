using FluentResults;

namespace Learnify.Catalog.WebApi.Features.CreateCourse;

public interface ICreateCourseUseCase
{
    Task<Result> ExecuteAsync(CreateCourseRequest request, CancellationToken cancellationToken = default);
}