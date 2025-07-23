namespace Learnify.Identity.WebApi.Features.Users.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByProviderKeyAsync(string providerKey, CancellationToken cancellationToken = default);
}
