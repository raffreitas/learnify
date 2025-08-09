using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.SubmitCourseForReview;

internal sealed class SubmitCourseForReviewUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : ISubmitCourseForReviewUseCase
{
    public async Task<Result> ExecuteAsync(SubmitCourseForReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.CourseId));

        course.SentToReview();

        await courseRepository.UpdateAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}