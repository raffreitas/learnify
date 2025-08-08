using Learnify.Courses.Domain.SeedWork;
using Learnify.Courses.Infrastructure.Persistence.Context;

namespace Learnify.Courses.Infrastructure.Persistence.Repositories;

internal abstract class RepositoryBase<TAggregate>(ApplicationDbContext dbContext)
    : IGenericRepository<TAggregate> where TAggregate : AggregateRoot
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TAggregate>().FindAsync([id], cancellationToken);
    }

    public async Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TAggregate>().AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TAggregate>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TAggregate>().Remove(entity);
        return Task.CompletedTask;
    }
}