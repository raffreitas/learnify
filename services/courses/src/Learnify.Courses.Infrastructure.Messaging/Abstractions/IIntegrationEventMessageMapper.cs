using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Infrastructure.Messaging.Abstractions;

namespace Learnify.Courses.Infrastructure.Messaging.Mappers;

public interface IIntegrationEventMessageMapper
{
    IMessage Map(IntegrationEvent integrationEvent);
}