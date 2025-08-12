using Learnify.Contracts.Abstractions;
using Learnify.Messaging.Abstractions;

namespace Learnify.Courses.Infrastructure.Messaging.Adapters;

public sealed class IntegrationEventMessageAdapter(IntegrationEvent integrationEvent) : IMessage
{
    public string MessageType => integrationEvent.GetType().Name;
    public object Payload => integrationEvent;
}