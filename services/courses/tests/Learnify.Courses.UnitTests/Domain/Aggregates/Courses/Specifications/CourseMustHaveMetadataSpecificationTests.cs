using Learnify.Courses.Domain.Aggregates.Courses.Specifications;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Specifications;

[Trait("UnitTests", "Domain - Specifications")]
public sealed class CourseMustHaveMetadataSpecificationTests(CourseTestFixture fixture) : IClassFixture<CourseTestFixture>
{
    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_True_When_Course_Has_Categories_And_Language))]
    public void IsSatisfiedBy_Should_Return_True_When_Course_Has_Categories_And_Language()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        course.AddCategory(CategoryId.Create(Guid.NewGuid()));
        var specification = new CourseMustHaveMetadataSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Categories))]
    public void IsSatisfiedBy_Should_Return_False_When_Course_Has_No_Categories()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        var specification = new CourseMustHaveMetadataSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Theory(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Language_Is_Invalid))]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void IsSatisfiedBy_Should_Return_False_When_Language_Is_Invalid(string invalidLanguage)
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        course.AddCategory(CategoryId.Create(Guid.NewGuid()));
        course.UpdateCourseInfo(null, null, null, invalidLanguage, null);
        var specification = new CourseMustHaveMetadataSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact(DisplayName = nameof(IsSatisfiedBy_Should_Return_False_When_Both_Categories_And_Language_Are_Missing))]
    public void IsSatisfiedBy_Should_Return_False_When_Both_Categories_And_Language_Are_Missing()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        course.UpdateCourseInfo(null, null, null, "", null);
        var specification = new CourseMustHaveMetadataSpecification();

        // Act
        var result = specification.IsSatisfiedBy(course);

        // Assert
        result.ShouldBeFalse();
    }
}
