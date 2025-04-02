using Learnify.Core;
using Learnify.Courses.Domain.Enums;

namespace Learnify.Courses.Domain.Entities;
public sealed class Course : AggregateRoot
{
    private readonly List<Category> _categories = [];
    private readonly List<Module> _modules = [];

    public Guid InstructorId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public ECourseStatus Status { get; private set; }
    public EDifficultyLevel DifficultyLevel { get; private set; }

    public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();
    public IReadOnlyCollection<Module> Modules => _modules.AsReadOnly();

    private Course() { }

    private Course(Guid instructorId, string title, string description, decimal price, string imageUrl, EDifficultyLevel difficultyLevel)
    {
        InstructorId = instructorId;
        Title = title;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
        Status = ECourseStatus.Draft;
        DifficultyLevel = difficultyLevel;
    }

    public static Course Create(Guid instructorId, string title, string description, decimal price, string imageUrl, EDifficultyLevel difficultyLevel)
    {
        var course = new Course(instructorId, title, description, price, imageUrl, difficultyLevel);
        return course;
    }

    public void AddCategory(Category category) 
        => _categories.Add(category);

    public void AddModule(Module module) 
        => _modules.Add(module);

    public void Publish()
    {
        Status = ECourseStatus.Published;
    }

    public void Delete()
    {
        Status = ECourseStatus.Deleted;
    }
}
