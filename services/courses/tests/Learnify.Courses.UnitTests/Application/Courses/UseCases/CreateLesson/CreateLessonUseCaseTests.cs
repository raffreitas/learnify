using Bogus;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.UseCases.CreateLesson;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.CreateLesson;

[Trait("UnitTests", "Application - UseCases")]
public class CreateLessonUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly Faker _faker = new();

    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly CreateLessonUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public CreateLessonUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new CreateLessonUseCase(_courseRepository, _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Create_Lesson_When_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourseWithModule();
        var moduleId = course.Modules.First().Id;

        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new CreateLessonRequest
        {
            CourseId = course.Id,
            ModuleId = moduleId,
            Title = _faker.Commerce.ProductName(),
            Description = _faker.Commerce.ProductDescription(),
            VideoUrl = _faker.Internet.Url(),
            Order = _faker.Random.Int(0, 10),
            IsPublic = true
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _courseRepository.Received(1).UpdateAsync(course, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        course.Modules.First().Lessons.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found()
    {
        // Arrange
        _courseRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        var request = new CreateLessonRequest
        {
            CourseId = Guid.NewGuid(),
            ModuleId = Guid.NewGuid(),
            Title = _faker.Commerce.ProductName(),
            Description = _faker.Commerce.ProductDescription(),
            VideoUrl = _faker.Internet.Url(),
            Order = 0,
            IsPublic = false
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}