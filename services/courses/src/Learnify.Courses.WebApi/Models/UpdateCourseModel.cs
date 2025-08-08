using Learnify.Courses.Application.Courses.UseCases.UpdateCourse;

namespace Learnify.Courses.WebApi.Models;

public sealed record UpdateCourseModel
{
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public decimal? Price { get; init; }
    public string? Language { get; init; }
    public string? DifficultyLevel { get; init; }
    public Guid[]? Categories { get; init; }

    public UpdateCourseRequest ToRequest(Guid courseId) => new()
    {
        CourseId = courseId,
        Description = Description,
        ImageUrl = ImageUrl,
        Price = Price,
        Language = Language,
        DifficultyLevel = DifficultyLevel,
        Categories = Categories
    };
}