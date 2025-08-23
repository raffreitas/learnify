using Learnify.Courses.Application.Courses.UseCases.CreateLesson;

namespace Learnify.Courses.WebApi.Models;

public sealed class CreateLessonModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Order { get; init; }
    public required bool IsPublic { get; init; }

    public CreateLessonRequest ToRequest(Guid courseId, Guid moduleId) => new()
    {
        CourseId = courseId,
        ModuleId = moduleId,
        Title = Title,
        Description = Description,
        Order = Order,
        IsPublic = IsPublic
    };
}