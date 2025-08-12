using Learnify.Contracts.Abstractions;

namespace Learnify.Contracts.Courses;

public sealed record CoursePublishedIntegrationEvent(
    Guid CourseId,
    Guid InstructorId,
    string Title,
    string Description,
    decimal Price,
    string Currency,
    string ImageUrl,
    string Language,
    string DifficultyLevel
) : IntegrationEvent
{
    public override int Version { get; } = 1;
}