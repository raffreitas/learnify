using Learnify.Core;

namespace Learnify.Courses.Domain.Entities;

public sealed class Module : Entity
{
    private readonly List<Lesson> _lessons = [];

    public Guid CourseId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public int Order { get; private set; }

    public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

    private Module() { }

    public Module(Guid courseId, string title, int order)
    {
        CourseId = courseId;
        Title = title;
        Order = order;
    }

    public void AddLesson(Lesson lesson)
    {
        _lessons.Add(lesson);
    }
}
