using Learnify.Courses.Application.Courses.UseCases.ReorderLessons;

namespace Learnify.Courses.WebApi.Models;

public sealed class ReorderLessonsModel
{
    // map of LessonId -> Order
    public required Dictionary<Guid, int> Positions { get; init; }

    public ReorderLessonsRequest ToRequest(Guid courseId, Guid moduleId) => new()
    {
        CourseId = courseId,
        ModuleId = moduleId,
        Positions = Positions
    };
}
