namespace Learnify.VideoProcessing.Domain.SeedWork;

public interface ISpecification<in T> where T : Entity
{
    bool IsSatisfiedBy(T entity);
}