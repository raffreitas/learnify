using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.ReorderLessons;

public interface IReorderLessonsUseCase
{
    Task<Result> ExecuteAsync(ReorderLessonsRequest request, CancellationToken cancellationToken = default);
}
