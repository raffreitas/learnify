using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;
internal sealed class CourseMustHaveMetadataSpecification : ISpecification<Course>
{
    public bool IsSatisfiedBy(Course entity)
    {
        return entity.Categories.Count != 0 &&
            string.IsNullOrWhiteSpace(entity.Language) == false;
    }
}
