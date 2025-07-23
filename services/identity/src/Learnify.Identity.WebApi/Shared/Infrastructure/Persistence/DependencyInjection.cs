using Learnify.Identity.WebApi.Shared.Infrastructure.Persistence.Factory;

namespace Learnify.Identity.WebApi.Shared.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();
        return services;
    }
}
