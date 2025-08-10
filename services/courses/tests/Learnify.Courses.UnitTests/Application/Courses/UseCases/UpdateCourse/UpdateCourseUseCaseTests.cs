using Bogus;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Categories.Errors;
using Learnify.Courses.Application.Courses.UseCases.UpdateCourse;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;
using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.UpdateCourse;

[Trait("UnitTests", "Application - UseCases")]
public class UpdateCourseUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly Faker _faker = new();

    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly UpdateCourseUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public UpdateCourseUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new UpdateCourseUseCase(_courseRepository, _categoryRepository, _unitOfWork);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Update_Course_Info_When_Valid))]
    public async Task ExecuteAsync_Should_Update_Course_Info_When_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourse();
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new UpdateCourseRequest
        {
            CourseId = course.Id,
            Description = _faker.Commerce.ProductDescription(),
            ImageUrl = _faker.Internet.Url(),
            Price = _faker.Random.Decimal(1, 500),
            Language = _faker.PickRandom("English", "Spanish", "Portuguese"),
            DifficultyLevel = nameof(DifficultyLevel.Advanced)
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _courseRepository.Received(1).UpdateAsync(course, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        course.Description.ShouldBe(request.Description);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found))]
    public async Task ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found()
    {
        // Arrange
        _courseRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();
        var request = new UpdateCourseRequest
        {
            CourseId = Guid.NewGuid(), Description = _faker.Commerce.ProductDescription()
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Fail_When_Course_InReview_Or_Deleted))]
    public async Task ExecuteAsync_Should_Fail_When_Course_InReview_Or_Deleted()
    {
        // Arrange
        var course = _fixture.CreateCourseWithStatus(CourseStatus.InReview);
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new UpdateCourseRequest
        {
            CourseId = course.Id, Description = _faker.Commerce.ProductDescription()
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Fail_When_Categories_Contain_Invalid))]
    public async Task ExecuteAsync_Should_Fail_When_Categories_Contain_Invalid()
    {
        // Arrange
        var course = _fixture.CreateValidCourse();
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);
        _categoryRepository.ExistsByIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var request = new UpdateCourseRequest { CourseId = course.Id, Categories = [Guid.NewGuid(), Guid.NewGuid()] };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBe(CategoriesErrors.CategoryNotFound("").Message);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Update_Categories_When_All_Valid))]
    public async Task ExecuteAsync_Should_Update_Categories_When_All_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourse();
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);
        _categoryRepository.ExistsByIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var categories = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var request = new UpdateCourseRequest { CourseId = course.Id, Categories = categories };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        course.Categories.Select(c => c.Value).ShouldBe(categories);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid))]
    public async Task ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid()
    {
        // Arrange
        var request = new UpdateCourseRequest { CourseId = Guid.Empty, Description = new string('x', 3) };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}