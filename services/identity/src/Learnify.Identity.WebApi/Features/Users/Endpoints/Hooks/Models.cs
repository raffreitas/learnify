namespace Learnify.Identity.WebApi.Features.Users.Endpoints.Hooks;

public sealed record CreateUserHookRequest
{
    public required string ProviderKey { get; init; }
    public required string ProviderType { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Picture { get; init; }
}
