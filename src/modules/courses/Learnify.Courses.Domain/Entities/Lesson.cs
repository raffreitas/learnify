using Learnify.Core;

namespace Learnify.Courses.Domain.Entities;

public sealed class Lesson : Entity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string VideoUrl { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public Guid ModuleId { get; private set; }

    private Lesson() { }

    public Lesson(Guid moduleId, string title, string description, string videoUrl, int order)
    {
        ModuleId = moduleId;
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        Order = order;
    }
}
