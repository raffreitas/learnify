namespace Learnify.Catalog.Core.Entities;

public sealed class Instructor
{
    public required Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string Bio { get; init; }
    public required string ImageUrl { get; init; }
}