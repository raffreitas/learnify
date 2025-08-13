using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.Options;

namespace Learnify.Messaging.RabbitMQ.Settings;

public sealed record RabbitMqMessageSettings
{
    public const string SectionName = nameof(RabbitMqMessageSettings);

    [Required, MinLength(1)] public required string ConnectionString { get; init; }

    [Required, MinLength(1), ValidateEnumeratedItems]
    public required Dictionary<string, MessageSettings> Messages { get; init; } = [];

    public MessageSettings GetMessageSettings(string messageKey)
    {
        return Messages.TryGetValue(messageKey, out var value)
            ? value
            : throw new InvalidOperationException();
    }
}

public sealed record MessageSettings
{
    [Required, MinLength(1)] public required string ExchangeName { get; init; }

    [Required, MinLength(1)] public required string ExchangeType { get; init; }

    [Required, MinLength(1)] public required string RoutingKey { get; init; }

    [Required, MinLength(1)] public required string QueueName { get; init; }

    public required bool Exclusive { get; init; } = false;

    public required bool Durable { get; init; } = true;
}