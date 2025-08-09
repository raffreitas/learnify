using Bogus;

using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses;

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
}