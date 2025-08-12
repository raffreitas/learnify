using Learnify.Contracts.Courses;
using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Domain.Aggregates.Courses.Events;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Application.Courses.Events;

public class CourseApprovedDomainEventHandler(IEventOutboxService eventOutboxService)
    : IDomainEventHandler<CourseApprovedDomainEvent>
{
    public async Task HandleAsync(CourseApprovedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var integrationEvent = new CourseApprovedIntegrationEvent(
            domainEvent.CourseId,
            domainEvent.InstructorId,
            domainEvent.Title
        );

        await eventOutboxService.AddAsync(integrationEvent, cancellationToken);
    }
}