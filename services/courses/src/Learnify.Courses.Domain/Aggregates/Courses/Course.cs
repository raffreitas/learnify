using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Aggregates.Courses.Specifications;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses;

public sealed class Course : AggregateRoot
{
    private readonly List<Module> _modules = [];
    private readonly List<CategoryId> _categories = [];

    public Guid InstructorId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public Price Price { get; private set; }
    public string Language { get; private set; }
    public CourseStatus Status { get; private set; }
    public DifficultyLevel DifficultyLevel { get; private set; }
    public IReadOnlyCollection<Module> Modules => _modules.AsReadOnly();
    public IReadOnlyCollection<CategoryId> Categories => _categories.AsReadOnly();

    public bool IsDraft => Status == CourseStatus.Draft;
    public bool IsPublished => Status == CourseStatus.Published;
    public bool IsInReview => Status == CourseStatus.InReview;
    public bool IsDeleted => Status == CourseStatus.Deleted;

    #region EF Constructor

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Course() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    #endregion

    private Course(
        Guid instructorId,
        string title,
        string description,
        string imageUrl,
        Price price,
        string language,
        DifficultyLevel difficultyLevel,
        CourseStatus status
    )
    {
        InstructorId = instructorId;
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        Price = price;
        Language = language;
        DifficultyLevel = difficultyLevel;
        Status = status;
    }

    public static Course Create(
        Guid instructorId,
        string title,
        string description,
        string imageUrl,
        Price price,
        string language,
        DifficultyLevel difficultyLevel,
        CourseStatus status
    )
    {
        var course = new Course(instructorId, title, description, imageUrl, price, language, difficultyLevel, status);

        var canBeCreatedSpecification = new CourseMustHaveBasicInfoSpecification();
        if (!canBeCreatedSpecification.IsSatisfiedBy(course))
            throw new DomainException("Course must have basic information.");

        return course;
    }

    public static Course CreateAsDraft(Guid instructorId, string title)
    {
        var course = new Course(
            instructorId,
            title,
            string.Empty,
            string.Empty,
            Price.Create(0),
            string.Empty,
            DifficultyLevel.Beginner,
            CourseStatus.Draft
        );

        var canBeCreatedAsDraftSpecification = new CourseCanBeCreatedAsDraftSpecification();
        if (!canBeCreatedAsDraftSpecification.IsSatisfiedBy(course))
            throw new DomainException("Course cannot be created as draft without content.");

        return course;
    }

    public void UpdateCourseInfo(
        string? description,
        string? imageUrl,
        Price? price,
        string? language,
        DifficultyLevel? difficultyLevel
    )
    {
        if (Status is CourseStatus.InReview or CourseStatus.Deleted)
            throw new DomainException("Unable to update information for this course");

        Description = description ?? Description;
        ImageUrl = imageUrl ?? ImageUrl;
        Price = price ?? Price;
        Language = language ?? Language;
        DifficultyLevel = difficultyLevel ?? DifficultyLevel;
    }

    public void AddModule(string title, int order)
    {
        if (Status is CourseStatus.InReview or CourseStatus.Deleted)
            throw new DomainException("Unable to add module for this course");

        var module = Module.Create(Id, title, order);

        _modules.Add(module);

        SentToReviewIfPublished();
    }

    public void AddLessonToModule(Guid moduleId, LessonInfo info)
    {
        if (Status is CourseStatus.InReview or CourseStatus.Deleted)
            throw new DomainException("Unable to add lesson for this course.");

        var module = _modules.FirstOrDefault(m => m.Id == moduleId)
                     ?? throw new DomainException("Module not found.");

        module.AddLesson(info);

        SentToReviewIfPublished();
    }

    public void ClearCategories()
    {
        if (Status is CourseStatus.InReview or CourseStatus.Deleted)
            throw new DomainException("Unable to update categories for this course.");

        _categories.Clear();
    }

    public void AddCategory(CategoryId category)
    {
        if (Status is CourseStatus.InReview or CourseStatus.Deleted)
            throw new DomainException("Unable to add category for this course.");

        _categories.Add(category);
    }

    public void RequestReview()
    {
        var canBeReviewedSpecification = new CourseCanBeSentForReviewSpecification();
        if (!canBeReviewedSpecification.IsSatisfiedBy(this))
            throw new DomainException("Course cannot be reviewed without content.");

        Status = CourseStatus.InReview;
        // TODO: Add domain event
    }

    public void Publish()
    {
        var canBePublishedSpecification = new CourseCanBePublishedSpecification();
        if (!canBePublishedSpecification.IsSatisfiedBy(this))
            throw new DomainException("Course cannot be published without content.");

        Status = CourseStatus.Published;
        // TODO: Add domain event
    }

    private void SentToReviewIfPublished()
    {
        if (Status == CourseStatus.Published)
            RequestReview();
    }
}