namespace Learnify.Catalog.Core.Entities;

public sealed class Category(Guid id, string name)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
}