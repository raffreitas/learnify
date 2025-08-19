using Learnify.Catalog.Core.Entities;
using Learnify.Catalog.Core.Enums;

namespace Learnify.Catalog.Core.Repositories;

public interface ICourseRepository
{
    Task AddAsync(Course course, CancellationToken cancellationToken = default);
    Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Course>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Course>> GetByInstructorIdAsync(
        Guid instructorId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<Course>> GetByCategoryIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<Course>> GetByLanguageAsync(
        string language,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<Course>> GetByDifficultyLevelAsync(
        DifficultyLevel difficultyLevel,
        CancellationToken cancellationToken = default
    );
}