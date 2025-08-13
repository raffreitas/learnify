using Learnify.Contracts.Abstractions;

namespace Learnify.Catalog.Application.Abstractions;

public interface IMessageBusService
{
    Task ConsumeAsync<TIntegrationEvent>(
        Func<TIntegrationEvent, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default
    ) where TIntegrationEvent : IntegrationEvent;
}