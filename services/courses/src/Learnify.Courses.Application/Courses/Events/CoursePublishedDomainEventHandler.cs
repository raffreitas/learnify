using Learnify.Contracts.Courses;
using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Domain.Aggregates.Courses.Events;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Application.Courses.Events;

public sealed class CoursePublishedDomainEventHandler(IEventOutboxService eventOutboxService)
    : IDomainEventHandler<CoursePublishedDomainEvent>
{
    public async Task HandleAsync(CoursePublishedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var integrationEvent = new CoursePublishedIntegrationEvent(
            domainEvent.CourseId,
            domainEvent.InstructorId,
            domainEvent.Title,
            domainEvent.Description,
            domainEvent.Price,
            domainEvent.Currency,
            domainEvent.ImageUrl,
            domainEvent.Language,
            domainEvent.DifficultyLevel
        );

        await eventOutboxService.AddAsync(integrationEvent, cancellationToken);
    }
}