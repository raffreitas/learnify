using System.Data;

namespace Learnify.Identity.WebApi.Shared.Infrastructure.Persistence.Factory;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnection(CancellationToken cancellationToken = default);
}
