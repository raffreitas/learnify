using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<bool> ExistsByIdAsync(Guid courseId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default);
}