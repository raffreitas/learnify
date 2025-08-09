using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.ReorderModules;

public interface IReorderModulesUseCase
{
    Task<Result> ExecuteAsync(ReorderModulesRequest request, CancellationToken cancellationToken = default);
}
