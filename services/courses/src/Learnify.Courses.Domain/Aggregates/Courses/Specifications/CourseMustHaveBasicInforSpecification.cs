using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;

internal sealed class CourseMustHaveBasicInfoSpecification : ISpecification<Course>
{
    public bool IsSatisfiedBy(Course entity)
    {
        return !string.IsNullOrWhiteSpace(entity.Title)
               && !string.IsNullOrWhiteSpace(entity.Description)
               && !string.IsNullOrWhiteSpace(entity.ImageUrl)
               && entity.DifficultyLevel != default
               && entity.Status != default
               && entity.Price != default;
    }
}