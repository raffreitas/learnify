using System.Text.Json;

using Learnify.Catalog.Core.Abstractions;
using Learnify.Catalog.Infrastructure.Messaging.Adapters;
using Learnify.Contracts.Abstractions;
using Learnify.Messaging.Abstractions;

namespace Learnify.Catalog.Infrastructure.Messaging.Services;

public class MessageBusService(IMessageConsumer messageConsumer) : IMessageBusService
{
    public async Task ConsumeAsync<TIntegrationEvent>(
        Func<TIntegrationEvent, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default
    ) where TIntegrationEvent : IntegrationEvent
    {
        await messageConsumer.ConsumeAsync<IntegrationEventMessageAdapter<TIntegrationEvent>>(
            async (msg, ct) =>
            {
                if (msg.Payload is not JsonElement jsonElement)
                    throw new InvalidOperationException("Received message payload is not a valid JsonElement.");

                var evt = jsonElement.Deserialize<TIntegrationEvent>();

                if (evt is null)
                    throw new InvalidOperationException(
                        $"Failed to deserialize message payload to {typeof(TIntegrationEvent).Name}."
                    );

                await handler(evt, ct);
            }, cancellationToken);
    }
}