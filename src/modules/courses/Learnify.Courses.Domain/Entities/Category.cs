using Learnify.Core;

namespace Learnify.Courses.Domain.Entities;

public sealed class Category : Entity
{
    public string Name { get; private set; } = string.Empty;

    private Category() { }

    public Category(string name)
    {
        Name = name;
    }
}
