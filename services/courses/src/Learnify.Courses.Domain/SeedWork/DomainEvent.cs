namespace Learnify.Courses.Domain.SeedWork;

public abstract record DomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
    public abstract Guid AggregateId { get; }
}