namespace Learnify.Courses.Application.Abstractions.Events.Abstractions;

public abstract record IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public abstract int Version { get; }
    public DateTimeOffset OccurredOn { get; protected set; } = DateTimeOffset.UtcNow;
};