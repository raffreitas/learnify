using Learnify.Catalog.Application.Abstractions;
using Learnify.Catalog.Infrastructure.Messaging.Services;
using Learnify.Messaging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learnify.Catalog.Infrastructure.Messaging;

public static class MessagingModule
{
    public static IServiceCollection AddMessagingModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddMessagingConfiguration(configuration);

        services.AddSingleton<IMessageBusService, MessageBusService>();
        return services;
    }
}