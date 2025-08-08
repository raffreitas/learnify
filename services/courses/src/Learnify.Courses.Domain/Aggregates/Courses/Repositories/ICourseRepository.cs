using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default);
}