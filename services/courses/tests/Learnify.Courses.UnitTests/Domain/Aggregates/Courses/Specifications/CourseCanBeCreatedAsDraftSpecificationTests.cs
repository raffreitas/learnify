using Learnify.Courses.Domain.Aggregates.Courses.Specifications;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Specifications;

[Trait("UnitTests", "Domain - Specifications")]
public sealed class CourseCanBeCreatedAsDraftSpecificationTests(CourseTestFixture fixture)
    : IClassFixture<CourseTestFixture>
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
}