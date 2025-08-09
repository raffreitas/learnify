using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.CreateModule;

public interface ICreateModuleUseCase
{
    Task<Result<CreateModuleResponse>> ExecuteAsync(CreateModuleRequest request,
        CancellationToken cancellationToken = default);
}