using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Infrastructure.Messaging.Abstractions;

namespace Learnify.Courses.Infrastructure.Messaging.Mappers;

public sealed class IntegrationEventMessageAdapter(IntegrationEvent integrationEvent) : IMessage
{
    public string MessageType => integrationEvent.GetType().Name;
    public object Payload => integrationEvent;
}