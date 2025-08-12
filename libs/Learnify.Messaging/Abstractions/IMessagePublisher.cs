namespace Learnify.Messaging.Abstractions;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default
    ) where TMessage : IMessage;
}