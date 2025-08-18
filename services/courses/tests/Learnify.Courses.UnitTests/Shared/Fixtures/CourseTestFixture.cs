using Bogus;

using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

namespace Learnify.Courses.UnitTests.Shared.Fixtures;

public sealed class CourseTestFixture
{
    public Faker Faker { get; } = new();

    public Course CreateValidCourse()
    {
        return Course.Create(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Commerce.ProductDescription(),
            Faker.Internet.Url(),
            Price.Create(Faker.Random.Decimal(10, 100)),
            Faker.Random.ArrayElement(["English", "Spanish", "Portuguese"]),
            Faker.PickRandom<DifficultyLevel>(),
            CourseStatus.Draft
        );
    }

    public Course CreateCourseWithStatus(CourseStatus status)
    {
        return Course.Create(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Commerce.ProductDescription(),
            Faker.Internet.Url(),
            Price.Create(Faker.Random.Decimal(10, 100)),
            Faker.Random.ArrayElement(["English", "Spanish", "Portuguese"]),
            Faker.PickRandom<DifficultyLevel>(),
            status
        );
    }

    public Course CreateValidCourseWithModule()
    {
        var course = CreateValidCourse();
        var module = Module.Create(
            course.Id,
            Faker.Commerce.ProductName(),
            Faker.Random.Int(1, 10)
        );
        course.AddModule(module);
        return course;
    }

    public Course CreateValidCourseWithModuleAndCategoryAndLesson()
    {
        var course = CreateValidCourseWithModule();
        var moduleId = course.Modules.First().Id;
        course.AddLessonToModule(moduleId, CreateLessonInfo());
        course.AddCategory(CategoryId.Create(Guid.NewGuid()));
        return course;
    }

    public LessonInfo CreateLessonInfo()
    {
        return new LessonInfo(
            Faker.Commerce.ProductName(),
            Faker.Commerce.ProductDescription(),
            Faker.Internet.Url(),
            Faker.Random.Int(1, 10),
            Faker.Random.Bool()
        );
    }

    /// <summary>
    /// Creates a course with invalid basic information for testing specifications.
    /// Uses reflection to bypass the validation in Course.Create()
    /// </summary>
    public Course CreateCourseWithInvalidBasicInfo(
        Guid? instructorId = null,
        string? title = null,
        string? description = null,
        string? imageUrl = null,
        Price? price = null,
        string? language = null,
        DifficultyLevel? difficultyLevel = null,
        CourseStatus? status = null)
    {
        // Create a valid course first
        var course = CreateValidCourse();

        // Use reflection to set invalid properties
        var type = typeof(Course);

        if (instructorId.HasValue)
            type.GetProperty(nameof(Course.InstructorId))?.SetValue(course, instructorId.Value);

        if (title != null)
            type.GetProperty(nameof(Course.Title))?.SetValue(course, title);

        if (description != null)
            type.GetProperty(nameof(Course.Description))?.SetValue(course, description);

        if (imageUrl != null)
            type.GetProperty(nameof(Course.ImageUrl))?.SetValue(course, imageUrl);

        if (price != null)
            type.GetProperty(nameof(Course.Price))?.SetValue(course, price);

        if (language != null)
            type.GetProperty(nameof(Course.Language))?.SetValue(course, language);

        if (difficultyLevel.HasValue)
            type.GetProperty(nameof(Course.DifficultyLevel))?.SetValue(course, difficultyLevel.Value);

        if (status.HasValue)
            type.GetProperty(nameof(Course.Status))?.SetValue(course, status.Value);

        return course;
    }
}