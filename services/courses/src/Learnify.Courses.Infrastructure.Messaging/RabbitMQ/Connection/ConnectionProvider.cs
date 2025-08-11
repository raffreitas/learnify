using RabbitMQ.Client;

namespace Learnify.Courses.Infrastructure.Messaging.RabbitMQ.Connection;

internal sealed class ConnectionProvider(IConnection consumerConnection, IConnection producerConnection)
{
    public IConnection ConsumerConnection { get; } = consumerConnection;
    public IConnection ProducerConnection { get; } = producerConnection;
}