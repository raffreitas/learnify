using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Domain.SeedWork;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Learnify.Courses.Infrastructure.Persistence.Interceptors;

public class SavingChangesInterceptor(IDomainEventDispatcher domainEventDispatcher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            await DispatchEventsIfNeeded(eventData.Context, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchEventsIfNeeded(DbContext dbContext, CancellationToken cancellationToken = default)
    {
        List<DomainEvent> domainEvents;
        do
        {
            domainEvents = GetAndClearDomainEventsFromAggregates(dbContext);

            if (domainEvents.Count != 0)
                await domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
        } while (domainEvents.Count != 0);
    }

    private static List<DomainEvent> GetAndClearDomainEventsFromAggregates(DbContext dbContext) =>
    [
        .. dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Count != 0)
            .SelectMany(x =>
            {
                IEnumerable<DomainEvent> domainEvents = [.. x.DomainEvents];
                x.ClearDomainEvents();
                return domainEvents;
            })
    ];
}