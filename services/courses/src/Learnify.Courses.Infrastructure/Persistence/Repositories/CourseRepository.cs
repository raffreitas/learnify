using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

namespace Learnify.Courses.Infrastructure.Persistence.Repositories;

internal sealed class CourseRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Course>(dbContext), ICourseRepository
{
    public new async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses
            .AsSplitQuery()
            .Include(x => x.Modules)
            .ThenInclude(x => x.Lessons)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses.AnyAsync(x => x.Id == courseId, cancellationToken);
    }

    public async Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses.AnyAsync(x => x.Title == title, cancellationToken);
    }
}