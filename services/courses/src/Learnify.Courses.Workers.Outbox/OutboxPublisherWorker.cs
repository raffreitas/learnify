using System.Text.Json;

using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Infrastructure.Messaging.Abstractions;
using Learnify.Courses.Infrastructure.Messaging.Mappers;
using Learnify.Courses.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

namespace Learnify.Courses.Workers.Outbox;

public class OutboxPublisherWorker(
    ILogger<OutboxPublisherWorker> logger,
    ITopologyInitializer topologyInitializer,
    IMessagePublisher messagePublisher,
    IServiceScopeFactory serviceScopeFactory
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await topologyInitializer.InitializeAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var eventMessageMapper = scope.ServiceProvider.GetRequiredService<IIntegrationEventMessageMapper>();

            var pendingEvents = await dbContext.EventOutbox
                .Where(x => x.ProcessedAt == null)
                .OrderBy(e => e.OccurredAt)
                .Take(100)
                .ToListAsync(stoppingToken);

            foreach (var outboxEvent in pendingEvents)
            {
                try
                {
                    var eventType = Type.GetType(outboxEvent.Type ?? "");
                    if (eventType is null || !typeof(IntegrationEvent).IsAssignableFrom(eventType))
                        throw new InvalidOperationException(
                            $"Type {outboxEvent.Type} is not a valid IntegrationEvent.");

                    var integrationEvent =
                        (IntegrationEvent?)JsonSerializer.Deserialize(outboxEvent.Content, eventType);
                    if (integrationEvent is null)
                        throw new InvalidOperationException(
                            $"Failed to deserialize event content for type {outboxEvent.Type}.");

                    var message = eventMessageMapper.Map(integrationEvent);
                    await messagePublisher.PublishAsync(message, stoppingToken);

                    outboxEvent.MarkProcessed();
                    logger.LogInformation("Published event {EventType} with id {EventId} via IMessagePublisher.",
                        outboxEvent.Type, outboxEvent.Id);
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Failed to publish event {EventType} with id {EventId}.", outboxEvent.Type,
                        outboxEvent.Id
                    );
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}