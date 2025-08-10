namespace Learnify.Courses.Infrastructure.Messaging.Abstractions;

public interface ITopologyInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}