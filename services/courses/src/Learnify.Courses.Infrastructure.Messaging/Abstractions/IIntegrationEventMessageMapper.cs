using Learnify.Contracts.Abstractions;
using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Messaging.Abstractions;

namespace Learnify.Courses.Infrastructure.Messaging.Abstractions;

public interface IIntegrationEventMessageMapper
{
    IMessage Map(IntegrationEvent integrationEvent);
}