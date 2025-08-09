using Learnify.Courses.Application.Courses.UseCases.UpdateLesson;

namespace Learnify.Courses.WebApi.Models;

public sealed class UpdateLessonModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string VideoUrl { get; init; }
    public required int Order { get; init; }
    public required bool IsPublic { get; init; }

    public UpdateLessonRequest ToRequest(Guid courseId, Guid moduleId, Guid lessonId) => new()
    {
        CourseId = courseId,
        ModuleId = moduleId,
        LessonId = lessonId,
        Title = Title,
        Description = Description,
        VideoUrl = VideoUrl,
        Order = Order,
        IsPublic = IsPublic
    };
}
