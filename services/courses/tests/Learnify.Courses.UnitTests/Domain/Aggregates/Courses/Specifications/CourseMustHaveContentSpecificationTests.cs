using Learnify.Courses.Domain.Aggregates.Courses.Specifications;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Specifications;

[Trait("UnitTests", "Domain - Specifications")]
public sealed class CourseMustHaveContentSpecificationTests(CourseTestFixture fixture) : IClassFixture<CourseTestFixture>
{
    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_True_When_Course_Has_Modules_With_Lessons))]
    public void IsSatisfiedBy_Should_Return_True_When_Course_Has_Modules_With_Lessons()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModule();
        var moduleId = course.Modules.First().Id;
        course.AddLessonToModule(moduleId, fixture.CreateLessonInfo());
        var specification = new CourseMustHaveContentSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Modules))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Modules()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        var specification = new CourseMustHaveContentSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Has_Modules_Without_Lessons))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Has_Modules_Without_Lessons()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModule();
        var specification = new CourseMustHaveContentSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Some_Modules_Have_No_Lessons))]
    public void IsSatisfiedBy_Should_Return_False_When_Some_Modules_Have_No_Lessons()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModule();
        var moduleId = course.Modules.First().Id;
        course.AddLessonToModule(moduleId, fixture.CreateLessonInfo());
        
        // Add another module without lessons
        var emptyModule = Learnify.Courses.Domain.Aggregates.Courses.Entities.Module.Create(
            course.Id,
            fixture.Faker.Commerce.ProductName(),
            2
        );
        course.AddModule(emptyModule);
        
        var specification = new CourseMustHaveContentSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }
}
