using Learnify.Contracts.Abstractions;

namespace Learnify.Contracts.Courses;

public sealed record CourseApprovedIntegrationEvent(Guid CourseId, Guid InstructorId, string Title) : IntegrationEvent
{
    public override int Version { get; } = 1;
}