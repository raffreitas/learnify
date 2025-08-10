using Learnify.Courses.Application.Abstractions.Events.Abstractions;

namespace Learnify.Courses.Application.Courses.Events;

public sealed record CoursePublishedIntegrationEvent : IntegrationEvent
{
    public override int Version { get; } = 1;
    public Guid CourseId { get; }
    public Guid InstructorId { get; }
    public string Title { get; }
    public decimal Price { get; }
    public string Currency { get; }
    public string Description { get; }
    public string ImageUrl { get; }
    public string Language { get; }
    public string DifficultyLevel { get; }

    public CoursePublishedIntegrationEvent(
        Guid courseId,
        Guid instructorId,
        string title,
        string description,
        decimal price,
        string currency,
        string imageUrl,
        string language,
        string difficultyLevel
    )
    {
        CourseId = courseId;
        InstructorId = instructorId;
        Title = title;
        Description = description;
        Price = price;
        Currency = currency;
        ImageUrl = imageUrl;
        Language = language;
        DifficultyLevel = difficultyLevel;
    }
}