using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Infrastructure.Messaging.Abstractions;

namespace Learnify.Courses.Infrastructure.Messaging.Mappers;

public sealed class IntegrationEventMessageMapper : IIntegrationEventMessageMapper
{
    public IMessage Map(IntegrationEvent integrationEvent)
    {
        return new IntegrationEventMessageAdapter(integrationEvent);
    }
}