using Learnify.Contracts.Courses;
using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Domain.Aggregates.Courses.Events;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Application.Courses.Events;

public sealed class RequestCourseReviewDomainEventHandler(
    IEventOutboxService eventOutboxService
) : IDomainEventHandler<RequestCourseReviewDomainEvent>
{
    public async Task HandleAsync(
        RequestCourseReviewDomainEvent domainEvent,
        CancellationToken cancellationToken = default
    )
    {
        var integrationEvent = new RequestCourseReviewIntegrationEvent(
            domainEvent.CourseId,
            domainEvent.InstructorId,
            domainEvent.Title,
            domainEvent.Description,
            domainEvent.Price
        );

        await eventOutboxService.AddAsync(integrationEvent, cancellationToken);
    }
}