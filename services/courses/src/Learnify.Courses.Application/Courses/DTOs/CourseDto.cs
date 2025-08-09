using Learnify.Courses.Application.Categories.DTOs;
using Learnify.Courses.Domain.Aggregates.Categories;
using Learnify.Courses.Domain.Aggregates.Courses;

namespace Learnify.Courses.Application.Courses.DTOs;

public record CourseDto
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; set; }
    public required string ImageUrl { get; init; }
    public required string Language { get; init; }
    public required string Status { get; init; }
    public required string DifficultyLevel { get; init; }
    public required CategoryDto[] Categories { get; init; } = [];
    public ModuleDto[] Modules { get; init; } = [];

    internal static CourseDto Create(Course course, Category[] categories) => new()
    {
        Id = course.Id,
        Title = course.Title,
        Description = course.Description,
        Price = course.Price,
        ImageUrl = course.ImageUrl,
        Language = course.Language,
        Currency = course.Price.Currency,
        Status = course.Status.ToString(),
        DifficultyLevel = course.DifficultyLevel.ToString(),
        Categories = categories.Select(CategoryDto.FromCategory).ToArray(),
        Modules = course.Modules.Select(ModuleDto.FromModule).ToArray(),
    };
}