using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Events;

public sealed record RequestCourseReviewDomainEvent : DomainEvent
{
    public override Guid AggregateId { get; }
    public Guid CourseId { get; }
    public Guid InstructorId { get; }
    public string Title { get; }
    public decimal Price { get; }
    public string Description { get; }

    private RequestCourseReviewDomainEvent(
        Guid courseId,
        Guid instructorId,
        string title,
        string description,
        decimal price
    )
    {
        AggregateId = courseId;
        CourseId = courseId;
        InstructorId = instructorId;
        Title = title;
        Description = description;
        Price = price;
    }

    public static RequestCourseReviewDomainEvent FromAggregate(Course course) => new(
        course.Id,
        course.InstructorId,
        course.Title,
        course.Description,
        course.Price
    );
}