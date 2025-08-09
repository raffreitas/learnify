using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.UpdateModule;

public interface IUpdateModuleUseCase
{
    Task<Result> ExecuteAsync(UpdateModuleRequest request, CancellationToken cancellationToken = default);
}
