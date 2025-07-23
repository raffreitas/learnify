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
}
