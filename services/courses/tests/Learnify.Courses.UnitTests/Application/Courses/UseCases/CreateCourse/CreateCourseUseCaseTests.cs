using Bogus;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.UseCases.CreateCourse;
using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

using NSubstitute;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.CreateCourse;

[Trait("UnitTests", "Application - UseCases")]
public class CreateCourseUseCaseTests
{
    private readonly Faker _faker = new();

    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly CreateCourseUseCase _useCase;

    public CreateCourseUseCaseTests()
    {
        _useCase = new CreateCourseUseCase(_courseRepository, _unitOfWork);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Create_Course_When_Title_Is_Unique))]
    public async Task ExecuteAsync_Should_Create_Course_When_Title_Is_Unique()
    {
        // Arrange
        var request = new CreateCourseRequest { Title = _faker.Commerce.ProductName() };
        _courseRepository.ExistsByTitleAsync(request.Title, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.CourseId.ShouldNotBe(Guid.Empty);
        await _courseRepository.Received(1).AddAsync(Arg.Any<Course>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Fail_When_Title_Already_Exists))]
    public async Task ExecuteAsync_Should_Fail_When_Title_Already_Exists()
    {
        // Arrange
        var request = new CreateCourseRequest { Title = _faker.Commerce.ProductName() };
        _courseRepository.ExistsByTitleAsync(request.Title, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
        await _courseRepository.DidNotReceive().AddAsync(Arg.Any<Course>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid))]
    public async Task ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid()
    {
        // Arrange
        var request = new CreateCourseRequest { Title = string.Empty };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}