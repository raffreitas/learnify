using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.Specifications;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Specifications;

[Trait("UnitTests", "Domain - Specifications")]
public sealed class CourseMustHaveBasicInfoSpecificationTests(CourseTestFixture fixture) : IClassFixture<CourseTestFixture>
{
    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_True_When_Course_Has_All_Basic_Information))]
    public void IsSatisfiedBy_Should_Return_True_When_Course_Has_All_Basic_Information()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        var specification = new CourseMustHaveBasicInfoSpecification();

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
        var specification = new CourseMustHaveBasicInfoSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Theory(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Description_Is_Invalid))]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void IsSatisfiedBy_Should_Return_False_When_Description_Is_Invalid(string invalidDescription)
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(description: invalidDescription);
        var specification = new CourseMustHaveBasicInfoSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Theory(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_ImageUrl_Is_Invalid))]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void IsSatisfiedBy_Should_Return_False_When_ImageUrl_Is_Invalid(string invalidImageUrl)
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(imageUrl: invalidImageUrl);
        var specification = new CourseMustHaveBasicInfoSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_InstructorId_Is_Empty))]
    public void IsSatisfiedBy_Should_Return_False_When_InstructorId_Is_Empty()
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(instructorId: Guid.Empty);
        var specification = new CourseMustHaveBasicInfoSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_DifficultyLevel_Is_Default))]
    public void IsSatisfiedBy_Should_Return_False_When_DifficultyLevel_Is_Default()
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(difficultyLevel: default(DifficultyLevel));
        var specification = new CourseMustHaveBasicInfoSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Status_Is_Default))]
    public void IsSatisfiedBy_Should_Return_False_When_Status_Is_Default()
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(status: default(CourseStatus));
        var specification = new CourseMustHaveBasicInfoSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Price_Is_Default))]
    public void IsSatisfiedBy_Should_Return_False_When_Price_Is_Default()
    {
        // Arrange
        var course = fixture.CreateCourseWithInvalidBasicInfo(price: default(Price)!);
        var specification = new CourseMustHaveBasicInfoSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }
}
