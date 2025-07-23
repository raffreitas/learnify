using Learnify.Identity.WebApi.Features.Users.Domain.Repositories;
using Learnify.Identity.WebApi.Features.Users.Endpoints.Hooks;
using Learnify.Identity.WebApi.Features.Users.Infrastructure.Repositories;

namespace Learnify.Identity.WebApi.Features.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersFeature(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IEndpointRouteBuilder UseUsersFeature(this WebApplication app)
    {
        app.MapUserHooksEndpoints();
        return app;
    }
}
