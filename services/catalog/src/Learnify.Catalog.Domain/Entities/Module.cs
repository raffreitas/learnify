namespace Learnify.Catalog.Core.Entities;

public sealed class Module
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required int Order { get; init; }
    public required IReadOnlyCollection<Lesson> Lessons { get; init; }
}