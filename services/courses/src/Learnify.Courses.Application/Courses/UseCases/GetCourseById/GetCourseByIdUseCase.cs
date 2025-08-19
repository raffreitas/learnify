using FluentResults;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Storage;
using Learnify.Courses.Application.Courses.Errors;
using Learnify.Courses.Application.Shared.Extensions;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;
using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.GetCourseById;

internal sealed class GetCourseByIdUseCase(
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository,
    IStorageService storageService
) : IGetCourseByIdUseCase
{
    public async Task<Result<GetCourseByIdResponse>> ExecuteAsync(
        GetCourseByIdRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var course = await courseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (course is null)
            return Result.Fail(CoursesErrors.CourseNotFound(request.Id));

        var categories = await categoryRepository.GetByIdsAsync(
            [..course.Categories.Select(x => x.Value)],
            cancellationToken
        );

        var response = GetCourseByIdResponse.FromAggregates(course, [..categories]);
        // TODO: Melhorar esse ponto para não macetar o storage.
        // var mediasUrls = await GetPresignedUrls(course, cancellationToken);
        // response = response with
        // {
        //     ImageUrl = mediasUrls.GetValueOrDefault(response.ImageUrl) ?? String.Empty,
        //     Modules = response.Modules.Select(module => module with
        //     {
        //         Lessons = module.Lessons.Select(lesson => lesson with
        //         {
        //             VideoUrl = mediasUrls.GetValueOrDefault(lesson.VideoUrl, lesson.VideoUrl)
        //         }).ToArray()
        //     }).ToArray()
        // };

        return response;
    }

    /*
    private async Task<Dictionary<string, string>> GetPresignedUrls(Course course, CancellationToken cancellationToken)
    {
        var imageTask = Task.Run(async () =>
        {
            var url = await storageService.GetFileUrlAsync(course.ImageUrl, cancellationToken: cancellationToken);
            return (Media: course.ImageUrl, Url: url);
        }, cancellationToken);

        var videoTasks = course.Modules
            .SelectMany(module => module.Lessons)
            .Select(async (lesson) =>
            {
                if (string.IsNullOrEmpty(lesson.VideoUrl))
                    return (Media: lesson.VideoUrl, Url: string.Empty);
                var url = await storageService.GetFileUrlAsync(
                    lesson.VideoUrl,
                    cancellationToken: cancellationToken
                );
                return (Media: lesson.VideoUrl, Url: url);
            });

        var urls = await Task.WhenAll([..videoTasks, imageTask]);

        return urls.ToDictionary(x => x.Media, x => x.Url);
    }
     */
}