namespace Learnify.Messaging.Abstractions;

public interface IMessageConsumer
{
    Task ConsumeAsync<TMessage>(
        Func<TMessage, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default
    ) where TMessage : IMessage;
}