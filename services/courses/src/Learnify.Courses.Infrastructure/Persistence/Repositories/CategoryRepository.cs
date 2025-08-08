using Learnify.Courses.Domain.Aggregates.Categories;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;
using Learnify.Courses.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

namespace Learnify.Courses.Infrastructure.Persistence.Repositories;

internal sealed class CategoryRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Category>(dbContext), ICategoryRepository
{
    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => _dbContext.Categories.AnyAsync(x => x.Name == name, cancellationToken);

    public Task<bool> ExistsByIdsAsync(IEnumerable<Guid> categoryIds, CancellationToken cancellationToken = default)
        => _dbContext.Categories
            .Where(x => categoryIds.Contains(x.Id))
            .AnyAsync(cancellationToken);
}