using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.UpdateLesson;

public interface IUpdateLessonUseCase
{
    Task<Result> ExecuteAsync(UpdateLessonRequest request, CancellationToken cancellationToken = default);
}
