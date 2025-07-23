using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Categories;

public sealed class Category : AggregateRoot
{
    private Category(string name)
        => Name = name;

    public string Name { get; private set; }

    public static Category Create(string name)
    {
        DomainException.ThrowIfNullOrWhitespace(name, nameof(name));
        return new Category(name);
    }
}
