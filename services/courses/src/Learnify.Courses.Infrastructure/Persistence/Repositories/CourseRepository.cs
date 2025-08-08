using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

namespace Learnify.Courses.Infrastructure.Persistence.Repositories;

internal sealed class CourseRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Course>(dbContext), ICourseRepository
{
    public async Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses.AnyAsync(x => x.Title == title, cancellationToken);
    }
}