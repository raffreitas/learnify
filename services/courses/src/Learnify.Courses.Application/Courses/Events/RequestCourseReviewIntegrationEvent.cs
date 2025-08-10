using Learnify.Courses.Application.Abstractions.Events.Abstractions;

namespace Learnify.Courses.Application.Courses.Events;

public sealed record RequestCourseReviewIntegrationEvent : IntegrationEvent
{
    public override int Version { get; } = 1;
    public Guid CourseId { get; }
    public Guid InstructorId { get; }
    public string Title { get; }
    public decimal Price { get; }
    public string Description { get; }

    public RequestCourseReviewIntegrationEvent(
        Guid courseId,
        Guid instructorId,
        string title,
        string description,
        decimal price
    )
    {
        CourseId = courseId;
        InstructorId = instructorId;
        Title = title;
        Description = description;
        Price = price;
    }
}