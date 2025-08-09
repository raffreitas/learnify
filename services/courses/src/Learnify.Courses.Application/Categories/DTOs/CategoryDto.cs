using Learnify.Courses.Domain.Aggregates.Categories;

namespace Learnify.Courses.Application.Categories.DTOs;

public sealed record CategoryDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }

    internal static CategoryDto FromCategory(Category category) => new() { Id = category.Id, Name = category.Name, };
}