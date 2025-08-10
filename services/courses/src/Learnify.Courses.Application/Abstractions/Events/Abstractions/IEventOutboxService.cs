namespace Learnify.Courses.Application.Abstractions.Events.Abstractions;

public interface IEventOutboxService
{
    Task AddAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IntegrationEvent;
}