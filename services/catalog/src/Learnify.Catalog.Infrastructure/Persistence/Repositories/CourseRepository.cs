using Learnify.Catalog.Core.Entities;
using Learnify.Catalog.Core.Enums;
using Learnify.Catalog.Core.Repositories;

using MongoDB.Driver;

namespace Learnify.Catalog.Infrastructure.Persistence.Repositories;

internal sealed class CourseRepository(IMongoDatabase database) : ICourseRepository
{
    private readonly IMongoCollection<Course> _collection = database.GetCollection<Course>("courses");

    public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
        => await _collection.InsertOneAsync(course, cancellationToken: cancellationToken);

    public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(
            c => c.Id == course.Id,
            course,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken
        );
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => await _collection.DeleteOneAsync(c => c.Id == id, cancellationToken);

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Course>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _collection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Course>> GetByInstructorIdAsync(
        Guid instructorId,
        CancellationToken cancellationToken = default
    )
    {
        return await _collection.Find(c => c.InstructorId == instructorId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Course>> GetByCategoryIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default
    )
    {
        return await _collection.Find(c => c.Categories.Any(cat => cat.Id == categoryId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Course>> GetByLanguageAsync(
        string language,
        CancellationToken cancellationToken = default
    )
    {
        return await _collection.Find(c => c.Language == language)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Course>> GetByDifficultyLevelAsync(
        DifficultyLevel difficultyLevel,
        CancellationToken cancellationToken = default
    )
    {
        return await _collection.Find(c => c.DifficultyLevel == difficultyLevel)
            .ToListAsync(cancellationToken);
    }
}