using Learnify.Courses.Domain.Aggregates.Courses.Specifications;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Specifications;

[Trait("UnitTests", "Domain - Specifications")]
public sealed class CourseCanBeSentForReviewSpecificationTests(CourseTestFixture fixture) : IClassFixture<CourseTestFixture>
{
    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_True_When_Course_Has_All_Required_Data))]
    public void IsSatisfiedBy_Should_Return_True_When_Course_Has_All_Required_Data()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModuleAndCategoryAndLesson();
        var specification = new CourseCanBeSentForReviewSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Basic_Info))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Basic_Info()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModule();
        var moduleId = course.Modules.First().Id;
        course.AddLessonToModule(moduleId, fixture.CreateLessonInfo());
        course.AddCategory(CategoryId.Create(Guid.NewGuid()));
        
        // Clear basic info by updating with empty values
        course.UpdateCourseInfo("", "", null, "", null);
        
        var specification = new CourseCanBeSentForReviewSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Metadata))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Metadata()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModule();
        var moduleId = course.Modules.First().Id;
        course.AddLessonToModule(moduleId, fixture.CreateLessonInfo());
        
        // Clear language (metadata requirement)
        course.UpdateCourseInfo(null, null, null, "", null);
        
        var specification = new CourseCanBeSentForReviewSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Content))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Content()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        course.AddCategory(CategoryId.Create(Guid.NewGuid()));
        var specification = new CourseCanBeSentForReviewSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Has_Modules_But_No_Lessons))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Has_Modules_But_No_Lessons()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModule();
        course.AddCategory(CategoryId.Create(Guid.NewGuid()));
        var specification = new CourseCanBeSentForReviewSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Multiple_Requirements_Are_Missing))]
    public void IsSatisfiedBy_Should_Return_False_When_Multiple_Requirements_Are_Missing()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        var specification = new CourseCanBeSentForReviewSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }
}
