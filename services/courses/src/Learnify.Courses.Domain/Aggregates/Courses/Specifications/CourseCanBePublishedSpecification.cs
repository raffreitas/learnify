using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;

internal sealed class CourseCanBePublishedSpecification : ISpecification<Course>
{
    private readonly CourseCanBeSentForReviewSpecification _internalSpec = new();

    public bool IsSatisfiedBy(Course entity)
        => _internalSpec.IsSatisfiedBy(entity)
           && entity is { IsRevised: true };
}