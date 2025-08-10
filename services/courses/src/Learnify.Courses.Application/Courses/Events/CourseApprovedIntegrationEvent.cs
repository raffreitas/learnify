using Learnify.Courses.Application.Abstractions.Events.Abstractions;

namespace Learnify.Courses.Application.Courses.Events;

public sealed record CourseApprovedIntegrationEvent : IntegrationEvent
{
    public override int Version { get; } = 1;
    public Guid CourseId { get; }
    public Guid InstructorId { get; }
    public string Title { get; }

    public CourseApprovedIntegrationEvent(Guid courseId, Guid instructorId, string title)
    {
        CourseId = courseId;
        InstructorId = instructorId;
        Title = title;
    }
}