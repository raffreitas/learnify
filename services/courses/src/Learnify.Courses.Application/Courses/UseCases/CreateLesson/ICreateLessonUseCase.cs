using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.CreateLesson;

public interface ICreateLessonUseCase
{
    Task<Result<CreateLessonResponse>> ExecuteAsync(CreateLessonRequest request, CancellationToken cancellationToken = default);
}
