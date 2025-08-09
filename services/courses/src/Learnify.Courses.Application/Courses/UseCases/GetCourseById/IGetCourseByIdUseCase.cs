using FluentResults;

using FluentValidation;

namespace Learnify.Courses.Application.Courses.UseCases.GetCourseById;

public interface IGetCourseByIdUseCase
{
    Task<Result<GetCourseByIdResponse>> ExecuteAsync(
        GetCourseByIdRequest request,
        CancellationToken cancellationToken = default
    );
}