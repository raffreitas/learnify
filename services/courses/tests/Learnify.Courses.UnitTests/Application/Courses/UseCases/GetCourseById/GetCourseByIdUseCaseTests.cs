using Bogus;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Storage;
using Learnify.Courses.Application.Courses.UseCases.GetCourseById;
using Learnify.Courses.Domain.Aggregates.Categories;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.GetCourseById;

[Trait("UnitTests", "Application - UseCases")]
public class GetCourseByIdUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly Faker _faker = new();

    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IStorageService _storageService = Substitute.For<IStorageService>();

    private readonly GetCourseByIdUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public GetCourseByIdUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new GetCourseByIdUseCase(_courseRepository, _categoryRepository, _storageService);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_Course_When_Found()
    {
        // Arrange
        var course = _fixture.CreateValidCourseWithModuleAndCategoryAndLesson();
        var module = course.Modules.First();
        var lesson = module.Lessons.First();

        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);
        _categoryRepository.GetByIdsAsync(Arg.Any<Guid[]>(), Arg.Any<CancellationToken>())
            .Returns([Category.Create(_faker.Commerce.Categories(1)[0])]);
        // FIXME: 
        // Storage returns presigned URLs
        // _storageService.GetFileUrlAsync(course.ImageUrl, Arg.Any<TimeSpan?>(), Arg.Any<CancellationToken>())
        //     .Returns(_faker.Internet.Url());
        // _storageService.GetFileUrlAsync(lesson.VideoUrl, Arg.Any<TimeSpan?>(), Arg.Any<CancellationToken>())
        //     .Returns(_faker.Internet.Url());

        var request = new GetCourseByIdRequest { Id = course.Id };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(course.Id);
        result.Value.Modules.Length.ShouldBe(course.Modules.Count);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found()
    {
        // Arrange
        _courseRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();
        var request = new GetCourseByIdRequest { Id = Guid.NewGuid() };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid()
    {
        // Arrange
        var request = new GetCourseByIdRequest { Id = Guid.Empty };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}