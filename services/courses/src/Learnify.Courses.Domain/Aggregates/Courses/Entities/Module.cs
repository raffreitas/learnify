using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Entities;

public sealed class Module : Entity
{
    private readonly List<Lesson> _lessons = [];

    public Guid CourseId { get; private set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

    private Module(Guid courseId, string title, int order)
    {
        CourseId = courseId;
        Title = title;
        Order = order;
    }

    public static Module Create(Guid courseId, string title, int order)
    {
        DomainException.ThrowIfNullOrWhitespace(title, nameof(title));
        DomainException.ThrowIfNullOrEmpty(title, nameof(title));
        DomainException.ThrowIfNegative(order, nameof(order));

        var module = new Module(courseId, title, order);
        return module;
    }

    public void AddLesson(LessonInfo info)
    {
        var lesson = Lesson.Create(Id, info);
        _lessons.Add(lesson);
    }

    public bool HasLessons() => _lessons.Count > 0;
}