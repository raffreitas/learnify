namespace Learnify.Courses.Infrastructure.Persistence.Models;

public sealed record OutboxMessage(string Type, string Content, DateTimeOffset OccurredAt)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Type { get; private set; } = Type;
    public string Content { get; private set; } = Content;
    public DateTimeOffset OccurredAt { get; private set; } = OccurredAt;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedAt { get; private set; }

    public void MarkProcessed()
    {
        ProcessedAt = DateTimeOffset.UtcNow;
    }
}