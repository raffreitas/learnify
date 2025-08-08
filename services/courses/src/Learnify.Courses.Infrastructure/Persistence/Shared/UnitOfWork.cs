using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Infrastructure.Persistence.Context;

namespace Learnify.Courses.Infrastructure.Persistence.Shared;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}