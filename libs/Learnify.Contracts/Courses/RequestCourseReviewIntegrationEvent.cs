using Learnify.Contracts.Abstractions;

namespace Learnify.Contracts.Courses;

public sealed record RequestCourseReviewIntegrationEvent(
    Guid CourseId,
    Guid InstructorId,
    string Title,
    string Description,
    decimal Price
) : IntegrationEvent
{
    public override int Version { get; } = 1;
}