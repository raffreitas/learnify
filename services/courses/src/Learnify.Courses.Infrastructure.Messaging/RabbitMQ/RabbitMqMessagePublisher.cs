using System.Diagnostics;
using System.Text.Json;

using Learnify.Courses.Infrastructure.Messaging.Abstractions;
using Learnify.Courses.Infrastructure.Messaging.RabbitMQ.Connection;
using Learnify.Courses.Infrastructure.Messaging.RabbitMQ.Settings;

using Microsoft.Extensions.Options;

using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

using RabbitMQ.Client;

namespace Learnify.Courses.Infrastructure.Messaging.RabbitMQ;

internal sealed class RabbitMqMessagePublisher(
    ChannelFactory channelFactory,
    IOptions<RabbitMqMessageSettings> options
) : IMessagePublisher
{
    private readonly RabbitMqMessageSettings _settings = options.Value;

    public async Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default
    ) where TMessage : IMessage
    {
        var messageType = message.MessageType;
        var messageSettings = _settings.GetMessageSettings(messageType);

        var properties = GetBasicProperties(messageType);

        #region Telemetry

        var activityName = $"{messageSettings.RoutingKey} publish";
        using var activity = MessagingDiagnostics.ActivitySource.StartActivity(activityName);

        ActivityContext contextToInject = default;

        if (activity != null)
            contextToInject = activity.Context;
        else if (Activity.Current != null)
            contextToInject = Activity.Current.Context;

        MessagingDiagnostics.Propagator.Inject(
            new PropagationContext(contextToInject, Baggage.Current),
            properties,
            InjectTraceContextIntoBasicProperties);

        SetActivityContext(activity, messageSettings.RoutingKey, "publish");

        #endregion

        await using var channel = await channelFactory.CreateForProducerAsync(cancellationToken);
        await channel.BasicPublishAsync(
            exchange: messageSettings.ExchangeName,
            routingKey: messageSettings.RoutingKey,
            mandatory: true,
            basicProperties: properties,
            body: JsonSerializer.SerializeToUtf8Bytes(message),
            cancellationToken: cancellationToken
        );
    }

    private static BasicProperties GetBasicProperties(string messageType) => new()
    {
        ContentType = "application/json",
        Persistent = true,
        MessageId = Guid.NewGuid().ToString(),
        Type = messageType,
        Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
        Headers = new Dictionary<string, object?>()
    };

    private static void InjectTraceContextIntoBasicProperties(IBasicProperties props, string key, string value)
    {
        props.Headers ??= new Dictionary<string, object?>();
        props.Headers[key] = value;
    }

    private static void SetActivityContext(Activity? activity, string routingKey, string operation)
    {
        if (activity is null)
            return;

        activity.SetTag("messaging.system", "rabbitmq");
        activity.SetTag("messaging.destination_kind", "exchange");
        activity.SetTag("messaging.operation", operation);
        activity.SetTag("messaging.destination.name", routingKey);
        activity.SetTag("messaging.rabbitmq.routing_key", routingKey);
    }
}