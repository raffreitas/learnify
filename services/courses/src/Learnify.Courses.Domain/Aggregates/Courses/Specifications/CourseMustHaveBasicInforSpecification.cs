using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;
internal sealed class CourseMustHaveBasicInfoSpecification : ISpecification<Course>
{
    public bool IsSatisfiedBy(Course entity)
    {
        return entity.Equals(default(Course)) == false
            && string.IsNullOrWhiteSpace(entity.Title) == false
            && string.IsNullOrWhiteSpace(entity.Description) == false
            && string.IsNullOrWhiteSpace(entity.ImageUrl) == false
            && entity.InstructorId != Guid.Empty
            && entity.DifficultyLevel != default
            && entity.Status != default
            && entity.Price != default;
    }
}
