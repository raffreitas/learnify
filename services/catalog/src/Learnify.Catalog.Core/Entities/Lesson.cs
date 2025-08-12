namespace Learnify.Catalog.Core.Entities;

public sealed class Lesson
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Order { get; init; }
}