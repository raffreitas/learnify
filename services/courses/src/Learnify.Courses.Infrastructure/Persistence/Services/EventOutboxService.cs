using System.Text.Json;

using Learnify.Contracts.Abstractions;
using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Infrastructure.Persistence.Context;
using Learnify.Courses.Infrastructure.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace Learnify.Courses.Infrastructure.Persistence.Services;

internal sealed class EventOutboxService(ApplicationDbContext dbContext) : IEventOutboxService
{
    public async Task AddAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IntegrationEvent
    {
        var outboxEvent = new EventOutbox(
            typeof(T).FullName!,
            JsonSerializer.Serialize(integrationEvent),
            integrationEvent.OccurredOn
        );

        await dbContext.EventOutbox.AddAsync(outboxEvent, cancellationToken);
    }
}