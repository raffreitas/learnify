using FluentResults;

namespace Learnify.Catalog.Application.UseCases.CreateCourse;

public interface ICreateCourseUseCase
{
    Task<Result> ExecuteAsync(CreateCourseRequest request, CancellationToken cancellationToken = default);
}