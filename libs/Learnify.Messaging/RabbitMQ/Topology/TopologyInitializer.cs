using Learnify.Messaging.Abstractions;
using Learnify.Messaging.RabbitMQ.Settings;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Learnify.Messaging.RabbitMQ.Topology;

internal sealed class RabbitMqTopologyInitializer(
    ConnectionFactory connectionFactory,
    IOptions<RabbitMqMessageSettings> options
) : ITopologyInitializer
{
    private readonly RabbitMqMessageSettings _settings = options.Value;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        foreach ((_, MessageSettings messageSettings) in _settings.Messages)
        {
            await channel.ExchangeDeclareAsync(
                exchange: messageSettings.ExchangeName,
                type: messageSettings.ExchangeType,
                durable: messageSettings.Durable,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken
            );

            await channel.QueueDeclareAsync(
                queue: messageSettings.QueueName,
                durable: messageSettings.Durable,
                exclusive: messageSettings.Exclusive,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken
            );

            await channel.QueueBindAsync(
                queue: messageSettings.QueueName,
                exchange: messageSettings.ExchangeName,
                routingKey: messageSettings.RoutingKey,
                arguments: null,
                cancellationToken: cancellationToken
            );
        }
    }
}