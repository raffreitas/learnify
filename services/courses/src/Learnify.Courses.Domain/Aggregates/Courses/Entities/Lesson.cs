using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Entities;

public sealed class Lesson : Entity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string VideoUrl { get; private set; }
    public int Order { get; private set; }
    public bool IsPublic { get; private set; }
    public Guid ModuleId { get; private set; }

    #region EF Constructor

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Lesson() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    #endregion

    private Lesson(Guid moduleId, string title, string description, string videoUrl, int order, bool isPublic)
    {
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        Order = order;
        IsPublic = isPublic;
        ModuleId = moduleId;
    }

    public static Lesson Create(Guid moduleId, LessonInfo info)
    {
        DomainException.ThrowIfNullOrWhitespace(info.Title, nameof(info.Title));
        DomainException.ThrowIfNullOrWhitespace(info.Description, nameof(info.Description));
        DomainException.ThrowIfNullOrWhitespace(info.VideoUrl, nameof(info.VideoUrl));
        DomainException.ThrowIfNegative(info.Order, nameof(info.Order));

        var lesson = new Lesson(moduleId, info.Title, info.Description, info.VideoUrl, info.Order, info.IsPublic);
        return lesson;
    }

    public void UpdateInfo(LessonInfo info)
    {
        DomainException.ThrowIfNullOrWhitespace(info.Title, nameof(info.Title));
        DomainException.ThrowIfNullOrWhitespace(info.Description, nameof(info.Description));
        DomainException.ThrowIfNullOrWhitespace(info.VideoUrl, nameof(info.VideoUrl));
        DomainException.ThrowIfNegative(info.Order, nameof(info.Order));

        Title = info.Title;
        Description = info.Description;
        VideoUrl = info.VideoUrl;
        Order = info.Order;
        IsPublic = info.IsPublic;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateOrder(int order)
    {
        DomainException.ThrowIfNegative(order, nameof(order));
        Order = order;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}