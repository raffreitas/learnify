namespace Learnify.Catalog.WebApi.Features.CreateCourse;

public sealed record CreateCourseRequest
{
    public required Guid CourseId { get; init; }
    public required Guid InstructorId { get; init; }
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
    public required string Description { get; init; }
    public required string ImageUrl { get; init; }
    public required string Language { get; init; }
    public required string DifficultyLevel { get; init; }
}