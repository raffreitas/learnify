namespace Learnify.Messaging.Abstractions;

public interface ITopologyInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}