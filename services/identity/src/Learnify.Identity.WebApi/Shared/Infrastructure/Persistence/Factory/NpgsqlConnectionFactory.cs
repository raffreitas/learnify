using System.Data;

using Npgsql;

namespace Learnify.Identity.WebApi.Shared.Infrastructure.Persistence.Factory;

public sealed class NpgsqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public const string ConnectionStringName = "DatabaseConnection";

    private readonly string _connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException("Connection string 'DatabaseConnection' not found.");

    public Task<IDbConnection> CreateConnection(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        return Task.FromResult<IDbConnection>(connection);
    }
}
