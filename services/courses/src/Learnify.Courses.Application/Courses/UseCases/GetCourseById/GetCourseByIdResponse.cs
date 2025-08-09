using Learnify.Courses.Application.Categories.DTOs;
using Learnify.Courses.Application.Courses.DTOs;
using Learnify.Courses.Domain.Aggregates.Categories;
using Learnify.Courses.Domain.Aggregates.Courses;

namespace Learnify.Courses.Application.Courses.UseCases.GetCourseById;

public sealed record GetCourseByIdResponse : CourseDto
{
    public static GetCourseByIdResponse FromAggregates(Course course, Category[] categories) => new()
    {
        Id = course.Id,
        Title = course.Title,
        Description = course.Description,
        Price = course.Price,
        Currency = course.Price.Currency,
        ImageUrl = course.ImageUrl,
        Language = course.Language,
        Status = course.Status.ToString(),
        DifficultyLevel = course.DifficultyLevel.ToString(),
        Categories = categories.Select(CategoryDto.FromCategory).ToArray(),
        Modules = course.Modules.Select(ModuleDto.FromModule).ToArray(),
    };
}