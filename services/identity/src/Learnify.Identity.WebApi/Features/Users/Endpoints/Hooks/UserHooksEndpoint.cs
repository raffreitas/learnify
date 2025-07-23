using Learnify.Identity.WebApi.Features.Users.Domain;
using Learnify.Identity.WebApi.Features.Users.Domain.Enums;
using Learnify.Identity.WebApi.Features.Users.Domain.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Learnify.Identity.WebApi.Features.Users.Endpoints.Hooks;

public static class UserHooksEndpoints
{
    public static IEndpointRouteBuilder MapUserHooksEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("users/hooks");
        group.MapPost("create", HandleUserCreated);
        return endpoints;
    }

    private static async Task<IResult> HandleUserCreated(
        [FromHeader(Name = "x-api-key")] string apiKey,
        [FromBody] CreateUserHookRequest request,
        [FromServices] IUserRepository userRepository,
        [FromServices] IConfiguration configuration
    )
    {
        if (apiKey != configuration["Hooks:ApiKey"])
            return Results.Unauthorized();

        var user = User.Create(
            request.ProviderKey,
            request.ProviderType,
            request.Email,
            UserRole.Student,
            request.FirstName,
            request.LastName,
            request.Picture
        );

        await userRepository.AddAsync(user);

        return Results.Created();
    }
}
