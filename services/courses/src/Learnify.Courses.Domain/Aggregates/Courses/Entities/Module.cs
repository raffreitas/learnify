using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Entities;

public sealed class Module : Entity
{
    private readonly List<Lesson> _lessons = [];

    public Guid CourseId { get; private set; }
    public string Title { get; private set; }
    public int Order { get; private set; }
    public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

    #region EF Constructor

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Module() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    #endregion
    
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

    public void UpdateInfo(string title, int order)
    {
        DomainException.ThrowIfNullOrWhitespace(title, nameof(title));
        DomainException.ThrowIfNegative(order, nameof(order));

        Title = title;
        Order = order;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public bool HasLessons() => _lessons.Count > 0;

    public void UpdateOrder(int order)
    {
        DomainException.ThrowIfNegative(order, nameof(order));
        Order = order;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ReorderLessons(Dictionary<Guid, int> positions)
    {
        if (positions.Count == 0) return;

        foreach (var lesson in _lessons)
        {
            if (positions.TryGetValue(lesson.Id, out var newOrder))
            {
                lesson.UpdateOrder(newOrder);
            }
        }

        // keep _lessons sorted by Order after updates
        _lessons.Sort((a, b) => a.Order.CompareTo(b.Order));
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}