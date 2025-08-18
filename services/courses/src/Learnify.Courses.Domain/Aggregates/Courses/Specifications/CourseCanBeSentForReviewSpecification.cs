using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;

internal sealed class CourseCanBeSentForReviewSpecification : ISpecification<Course>
{
    private readonly List<ISpecification<Course>> _specifications =
    [
        new CourseMustHaveBasicInfoSpecification(),
        new CourseMustHaveMetadataSpecification(),
        new CourseMustHaveContentSpecification()
    ];

    public bool IsSatisfiedBy(Course entity)
    {
        return _specifications.All(spec => spec.IsSatisfiedBy(entity));
    }
}