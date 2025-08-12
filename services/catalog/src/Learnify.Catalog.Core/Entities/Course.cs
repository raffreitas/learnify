using Learnify.Catalog.Core.Enums;

namespace Learnify.Catalog.Core.Entities;

public sealed class Course
{
    public required Guid Id { get; init; }
    public required Guid InstructorId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string ImageUrl { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
    public required string Language { get; init; }
    public required DifficultyLevel DifficultyLevel { get; init; }
    public required IReadOnlyCollection<Module> Modules { get; init; }
    public required IReadOnlyCollection<Category> Categories { get; init; }
    public required Instructor Instructor { get; init; }
    public bool IsListed { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}