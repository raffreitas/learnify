using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Categories.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    Task<bool> ExistsByIdsAsync(
        IEnumerable<Guid> categoryIds,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<Category>> GetByIdsAsync(
        Guid[] categoryIds,
        CancellationToken cancellationToken = default
    );
}