using Learnify.Courses.Domain.Aggregates.Courses.Specifications;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Specifications;

[Trait("UnitTests", "Domain - Specifications")]
public sealed class CourseCanBePublishedSpecificationTests(CourseTestFixture fixture) : IClassFixture<CourseTestFixture>
{
    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_True_When_Course_Is_Revised_And_Ready_For_Review))]
    public void IsSatisfiedBy_Should_Return_True_When_Course_Is_Revised_And_Ready_For_Review()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModuleAndCategoryAndLesson();
        course.RequestReview();
        course.ApproveForPublish(); // This sets IsRevised to true
        var specification = new CourseCanBePublishedSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Is_Not_Revised))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Is_Not_Revised()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModuleAndCategoryAndLesson();
        var specification = new CourseCanBePublishedSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Lacks_Basic_Info))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Lacks_Basic_Info()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModuleAndCategoryAndLesson();
        course.RequestReview();
        course.ApproveForPublish();
        
        // Clear basic info using the helper method
        var invalidCourse = fixture.CreateCourseWithInvalidBasicInfo(description: "");
        
        var specification = new CourseCanBePublishedSpecification();

        // Act
        var result = specification.IsSatisfiedBy(invalidCourse);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Lacks_Content))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Lacks_Content()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        course.AddCategory(CategoryId.Create(Guid.NewGuid()));
        var specification = new CourseCanBePublishedSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Lacks_Metadata))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Lacks_Metadata()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModule();
        var moduleId = course.Modules.First().Id;
        course.AddLessonToModule(moduleId, fixture.CreateLessonInfo());
        
        // Clear language (metadata requirement) using helper method
        var invalidCourse = fixture.CreateCourseWithInvalidBasicInfo(language: "");
        
        var specification = new CourseCanBePublishedSpecification();

        // Act
        var result = specification.IsSatisfiedBy(invalidCourse);

        // Assert
        result.ShouldBeFalse();
    }
}
