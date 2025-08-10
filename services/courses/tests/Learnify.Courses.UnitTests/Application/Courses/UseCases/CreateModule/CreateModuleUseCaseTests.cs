using Bogus;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.UseCases.CreateModule;
using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.CreateModule;

[Trait("UnitTests", "Application - UseCases")]
public class CreateModuleUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly Faker _faker = new();

    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly CreateModuleUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public CreateModuleUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new CreateModuleUseCase(_courseRepository, _unitOfWork);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Create_Module_When_Valid))]
    public async Task ExecuteAsync_Should_Create_Module_When_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourse();
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new CreateModuleRequest
        {
            CourseId = course.Id, Title = _faker.Commerce.ProductName(), Order = 0
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _courseRepository.Received(1).UpdateAsync(course, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        course.Modules.ShouldNotBeEmpty();
        course.Modules.First().Title.ShouldBe(request.Title);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found))]
    public async Task ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found()
    {
        // Arrange
        _courseRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();
        var request = new CreateModuleRequest
        {
            CourseId = Guid.NewGuid(), Title = _faker.Commerce.ProductName(), Order = 0
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid))]
    public async Task ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid()
    {
        // Arrange
        var request = new CreateModuleRequest { CourseId = Guid.Empty, Title = string.Empty, Order = -1 };

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

        var request = new CreateModuleRequest
        {
            CourseId = course.Id, Title = _faker.Commerce.ProductName(), Order = 0
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Fail_When_Module_Title_Already_Exists))]
    public async Task ExecuteAsync_Should_Fail_When_Module_Title_Already_Exists()
    {
        // Arrange
        var course = _fixture.CreateValidCourse();
        var existing = Module.Create(course.Id, _faker.Commerce.ProductName(), 0);
        course.AddModule(existing);
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new CreateModuleRequest { CourseId = course.Id, Title = existing.Title, Order = 1 };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}