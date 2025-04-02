namespace Learnify.Core;

public abstract class Entity
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
