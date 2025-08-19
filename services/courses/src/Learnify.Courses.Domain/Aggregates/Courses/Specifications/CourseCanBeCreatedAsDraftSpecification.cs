using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;

internal sealed class CourseCanBeCreatedAsDraftSpecification : ISpecification<Course>
{
    public bool IsSatisfiedBy(Course entity)
    {
        return !string.IsNullOrWhiteSpace(entity.Title);
    }
}