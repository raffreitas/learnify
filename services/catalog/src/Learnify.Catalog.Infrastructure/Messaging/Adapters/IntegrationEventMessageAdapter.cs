using Learnify.Contracts.Abstractions;
using Learnify.Messaging.Abstractions;

namespace Learnify.Catalog.Infrastructure.Messaging.Adapters;

public sealed class IntegrationEventMessageAdapter<TIntegrationEvent> : IMessage
    where TIntegrationEvent : IntegrationEvent
{
    public string MessageType => typeof(TIntegrationEvent).Name;
    public required object Payload { get; init; }
}