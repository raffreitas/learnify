using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.UseCases.RequestCourseReview;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.RequestCourseReview;

[Trait("UnitTests", "Application - UseCases")]
public class RequestCourseReviewUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly RequestCourseReviewUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public RequestCourseReviewUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new RequestCourseReviewUseCase(_courseRepository, _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Set_InReview_When_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourseWithModuleAndCategoryAndLesson();
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new RequestCourseReviewRequest { CourseId = course.Id };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        course.IsInReview.ShouldBeTrue();
        await _courseRepository.Received(1).UpdateAsync(course, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found()
    {
        // Arrange
        _courseRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();
        var request = new RequestCourseReviewRequest { CourseId = Guid.NewGuid() };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid()
    {
        // Arrange
        var request = new RequestCourseReviewRequest { CourseId = Guid.Empty };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}