using Bogus;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.UseCases.UpdateLesson;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.UpdateLesson;

[Trait("UnitTests", "Application - UseCases")]
public class UpdateLessonUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly Faker _faker = new();

    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly UpdateLessonUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public UpdateLessonUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new UpdateLessonUseCase(_courseRepository, _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Update_Lesson_When_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourseWithModule();
        var module = course.Modules.First();
        course.AddLessonToModule(module.Id, _fixture.CreateLessonInfo());
        var lesson = module.Lessons.First();

        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new UpdateLessonRequest
        {
            CourseId = course.Id,
            ModuleId = module.Id,
            LessonId = lesson.Id,
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
        var savedLesson = course.Modules.First().Lessons.First();
        savedLesson.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found()
    {
        // Arrange
        _courseRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        var request = new UpdateLessonRequest
        {
            CourseId = Guid.NewGuid(),
            ModuleId = Guid.NewGuid(),
            LessonId = Guid.NewGuid(),
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