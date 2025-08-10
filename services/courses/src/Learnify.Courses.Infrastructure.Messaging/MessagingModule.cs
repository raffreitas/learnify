using Learnify.Courses.Infrastructure.Messaging.Abstractions;
using Learnify.Courses.Infrastructure.Messaging.Mappers;
using Learnify.Courses.Infrastructure.Messaging.RabbitMQ;
using Learnify.Courses.Infrastructure.Messaging.RabbitMQ.Connection;
using Learnify.Courses.Infrastructure.Messaging.RabbitMQ.Settings;
using Learnify.Courses.Infrastructure.Messaging.RabbitMQ.Topology;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OpenLibs.Extensions;

using RabbitMQ.Client;

namespace Learnify.Courses.Infrastructure.Messaging;

public static class MessagingModule
{
    public static IServiceCollection AddMessagingModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.UseRabbitMq(configuration);
        services.AddScoped<IIntegrationEventMessageMapper, IntegrationEventMessageMapper>();
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