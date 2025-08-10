using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Events;

public sealed record CourseApprovedDomainEvent : DomainEvent
{
    public override Guid AggregateId { get; }
    public Guid CourseId { get; }
    public Guid InstructorId { get; }
    public string Title { get; }

    private CourseApprovedDomainEvent(Guid courseId, Guid instructorId, string title)
    {
        AggregateId = courseId;
        CourseId = courseId;
        InstructorId = instructorId;
        Title = title;
    }

    public static CourseApprovedDomainEvent FromAggregate(Course course) => new(
        course.Id,
        course.InstructorId,
        course.Title
    );
}