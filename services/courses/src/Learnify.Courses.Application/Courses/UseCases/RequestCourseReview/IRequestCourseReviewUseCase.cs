using FluentResults;

namespace Learnify.Courses.Application.Courses.UseCases.RequestCourseReview;

public interface IRequestCourseReviewUseCase
{
    Task<Result> ExecuteAsync(RequestCourseReviewRequest request, CancellationToken cancellationToken = default);
}