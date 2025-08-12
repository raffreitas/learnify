using Learnify.Messaging.Abstractions;
using Learnify.Messaging.RabbitMQ;
using Learnify.Messaging.RabbitMQ.Connection;
using Learnify.Messaging.RabbitMQ.Settings;
using Learnify.Messaging.RabbitMQ.Topology;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OpenLibs.Extensions;

using RabbitMQ.Client;

namespace Learnify.Messaging;

public static class MessagingModule
{
    public static IServiceCollection AddMessagingConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.UseRabbitMq(configuration);
        return services;
    }

    private static void UseRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.ConfigureRequiredSettings<RabbitMqMessageSettings>(
            configuration,
            RabbitMqMessageSettings.SectionName
        );

        var connectionFactory = new ConnectionFactory { Uri = new Uri(settings.ConnectionString) };

        var channelFactoryTask = ChannelFactory.CreateAsync(connectionFactory);
        channelFactoryTask.Wait();
        var channelFactory = channelFactoryTask.Result;

        services.AddTransient(_ => channelFactory);

        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddSingleton<IMessageConsumer, RabbitMqMessageConsumer>();
        services.AddSingleton<ITopologyInitializer, RabbitMqTopologyInitializer>(provider =>
            new RabbitMqTopologyInitializer(connectionFactory,
                provider.GetRequiredService<IOptions<RabbitMqMessageSettings>>()));
    }
}