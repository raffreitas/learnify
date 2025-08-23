namespace Learnify.VideoProcessing.Domain.SeedWork;

public interface IDomainEventHandler<in T> where T : DomainEvent
{
    Task HandleAsync(T domainEvent, CancellationToken cancellationToken = default);
}