using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Application.Abstractions.Events.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}