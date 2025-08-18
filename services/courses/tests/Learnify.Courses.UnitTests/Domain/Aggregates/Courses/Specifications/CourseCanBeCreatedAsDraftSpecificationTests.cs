using Learnify.Courses.Domain.Aggregates.Courses.Specifications;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Specifications;

[Trait("UnitTests", "Domain - Specifications")]
public sealed class CourseCanBeCreatedAsDraftSpecificationTests(CourseTestFixture fixture) : IClassFixture<CourseTestFixture>
{
    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_True_When_Course_Has_Valid_Instructor_And_Title))]
    public void IsSatisfiedBy_Should_Return_True_When_Course_Has_Valid_Instructor_And_Title()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        var specification = new CourseCanBeCreatedAsDraftSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_InstructorId_Is_Empty))]
    public void IsSatisfiedBy_Should_Return_False_When_InstructorId_Is_Empty()
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(instructorId: Guid.Empty);
        var specification = new CourseCanBeCreatedAsDraftSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Theory(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Title_Is_Invalid))]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void IsSatisfiedBy_Should_Return_False_When_Title_Is_Invalid(string invalidTitle)
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(title: invalidTitle);
        var specification = new CourseCanBeCreatedAsDraftSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Both_InstructorId_And_Title_Are_Invalid))]
    public void IsSatisfiedBy_Should_Return_False_When_Both_InstructorId_And_Title_Are_Invalid()
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(
            instructorId: Guid.Empty, 
            title: string.Empty);
        var specification = new CourseCanBeCreatedAsDraftSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }
}
