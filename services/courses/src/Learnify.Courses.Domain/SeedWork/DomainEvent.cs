namespace Learnify.Courses.Domain.SeedWork;

public abstract class DomainEvent
{
    public DateTimeOffset OccurredOn { get; set; } = DateTimeOffset.UtcNow;
}
