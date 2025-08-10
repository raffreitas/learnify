using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Events;

public sealed record CoursePublishedDomainEvent : DomainEvent
{
    public override Guid AggregateId { get; }
    public Guid CourseId { get; }
    public Guid InstructorId { get; }
    public string Title { get; }
    public decimal Price { get; }
    public string Currency { get; }
    public string Description { get; }
    public string ImageUrl { get; }
    public string Language { get; }
    public string DifficultyLevel { get; }

    private CoursePublishedDomainEvent(
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
        AggregateId = courseId;
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

    public static CoursePublishedDomainEvent FromAggregate(Course course) => new(
        course.Id,
        course.InstructorId,
        course.Title,
        course.Description,
        course.Price,
        course.Price.Currency,
        course.ImageUrl,
        course.Language,
        course.DifficultyLevel.ToString()
    );
}