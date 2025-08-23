using Learnify.VideoProcessing.Application.Abstractions;
using Learnify.VideoProcessing.Infrastructure.Persistence.Context;

namespace Learnify.VideoProcessing.Infrastructure.Persistence.Shared;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}