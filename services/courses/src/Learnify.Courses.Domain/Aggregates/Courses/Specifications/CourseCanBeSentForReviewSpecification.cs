using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;
internal sealed class CourseCanBeSentForReviewSpecification : ISpecification<Course>
{
    private readonly CourseCanBePublishedSpecification _internalSpec = new();
    public bool IsSatisfiedBy(Course entity)
        => _internalSpec.IsSatisfiedBy(entity);
}
