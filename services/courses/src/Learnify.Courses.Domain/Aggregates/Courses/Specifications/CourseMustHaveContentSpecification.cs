using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Specifications;
internal sealed class CourseMustHaveContentSpecification : ISpecification<Course>
{
    public bool IsSatisfiedBy(Course entity)
    {
        return entity.Modules.Count != 0
            && entity.Modules.All(module => module.HasLessons());
    }
}
