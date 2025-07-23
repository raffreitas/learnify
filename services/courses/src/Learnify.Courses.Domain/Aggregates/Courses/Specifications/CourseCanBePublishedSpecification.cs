using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;

internal sealed class CourseCanBePublishedSpecification : ISpecification<Course>
{
    private readonly List<ISpecification<Course>> _specifications;
    public CourseCanBePublishedSpecification()
    {
        _specifications =
        [
            new CourseMustHaveBasicInfoSpecification(),
            new CourseMustHaveMetadataSpecification(),
            new CourseMustHaveContentSpecification()
        ];
    }

    public bool IsSatisfiedBy(Course entity)
        => _specifications.All(spec => spec.IsSatisfiedBy(entity));
}
