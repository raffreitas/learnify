using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.SubmitCourseForReview;

public interface ISubmitCourseForReviewUseCase
{
    Task<Result> ExecuteAsync(SubmitCourseForReviewRequest request, CancellationToken cancellationToken = default);
}