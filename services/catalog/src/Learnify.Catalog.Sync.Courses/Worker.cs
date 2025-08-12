using Learnify.Catalog.Core.Abstractions;
using Learnify.Contracts.Courses;
using Learnify.Messaging.Abstractions;

namespace Learnify.Catalog.Sync.Courses;

internal sealed class Worker(
    ILogger<Worker> logger,
    IMessageBusService messageBusService,
    ITopologyInitializer topologyInitializer
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await topologyInitializer.InitializeAsync(stoppingToken);

        await messageBusService.ConsumeAsync<CoursePublishedIntegrationEvent>((@event, ct) =>
        {
            logger.LogInformation(
                "CouresPublished: {CourseId}, Title: {Title}, Instructor: {InstructorId}",
                @event.CourseId,
                @event.Title,
                @event.InstructorId
            );
            return Task.CompletedTask;
        }, stoppingToken);
    }
}