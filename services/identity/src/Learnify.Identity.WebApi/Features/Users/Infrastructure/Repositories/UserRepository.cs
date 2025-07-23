using System.Data;

using Dapper;

using Learnify.Identity.WebApi.Features.Users.Domain;
using Learnify.Identity.WebApi.Features.Users.Domain.Repositories;
using Learnify.Identity.WebApi.Shared.Infrastructure.Persistence.Factory;

namespace Learnify.Identity.WebApi.Features.Users.Infrastructure.Repositories;

public sealed class UserRepository(IDbConnectionFactory dbConnectionFactory) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql =
            """
            insert into public.users(id, provider_key, provider_type, email, first_name, last_name, role, is_active, picture, created_at, updated_at)
            values (@Id, @ProviderKey, @ProviderType, @Email, @FirstName, @LastName, @Role, @IsActive, @Picture, @CreatedAt, @UpdatedAt)
            """;

        using var connection = await dbConnectionFactory.CreateConnection(cancellationToken);
        var parameters = new DynamicParameters();
        parameters.Add("Id", user.Id, DbType.Guid);
        parameters.Add("ProviderKey", user.ProviderKey, DbType.String);
        parameters.Add("ProviderType", user.ProviderType, DbType.String);
        parameters.Add("Email", user.Email, DbType.String);
        parameters.Add("FirstName", user.FirstName, DbType.String);
        parameters.Add("LastName", user.LastName, DbType.String);
        parameters.Add("Role", user.Role.ToString(), DbType.AnsiString);
        parameters.Add("IsActive", user.IsActive, DbType.Boolean);
        parameters.Add("Picture", user.Picture, DbType.String);
        parameters.Add("CreatedAt", user.CreatedAt, DbType.DateTimeOffset);
        parameters.Add("UpdatedAt", user.UpdatedAt, DbType.DateTimeOffset);

        await connection.ExecuteAsync(sql, parameters);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql =
            """
            select id, provider_key, provider_type, email, first_name, last_name, role, is_active, picture, created_at, updated_at
            from public.users
            where id = @Id
            """;

        using var connection = await dbConnectionFactory.CreateConnection(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByProviderKeyAsync(string providerKey, CancellationToken cancellationToken = default)
    {
        const string sql =
            """
            select id, provider_key, provider_type, email, first_name, last_name, role, is_active, picture, created_at, updated_at
            from public.users
            where provider_key = @ProviderKey
            """;

        using var connection = await dbConnectionFactory.CreateConnection(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { ProviderKey = providerKey });
    }
}
