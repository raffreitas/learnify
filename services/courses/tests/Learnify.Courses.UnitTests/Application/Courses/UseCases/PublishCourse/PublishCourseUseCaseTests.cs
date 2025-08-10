using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.UseCases.PublishCourse;
using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.PublishCourse;

[Trait("UnitTests", "Application - UseCases")]
public class PublishCourseUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly PublishCourseUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public PublishCourseUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new PublishCourseUseCase(_courseRepository, _unitOfWork);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Publish_When_Valid))]
    public async Task ExecuteAsync_Should_Publish_When_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourseWithModuleAndCategoryAndLesson();
        course.RequestReview();
        course.ApproveForPublish();
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);
        var request = new PublishCourseRequest { CourseId = course.Id };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        course.IsPublished.ShouldBeTrue();
        await _courseRepository.Received(1).UpdateAsync(course, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found))]
    public async Task ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found()
    {
        // Arrange
        _courseRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();
        var request = new PublishCourseRequest { CourseId = Guid.NewGuid() };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid))]
    public async Task ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid()
    {
        // Arrange
        var request = new PublishCourseRequest { CourseId = Guid.Empty };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}