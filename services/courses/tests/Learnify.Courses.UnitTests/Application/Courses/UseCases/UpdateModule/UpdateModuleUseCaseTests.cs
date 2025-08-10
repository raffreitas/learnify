using Bogus;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.UseCases.UpdateModule;
using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.UpdateModule;

[Trait("UnitTests", "Application - UseCases")]
public class UpdateModuleUseCaseTests
{
    private readonly Faker _faker = new();

    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly UpdateModuleUseCase _useCase;

    public UpdateModuleUseCaseTests()
    {
        _useCase = new UpdateModuleUseCase(_courseRepository, _unitOfWork);
    }


    [Fact]
    public async Task ExecuteAsync_Should_Update_Module_When_Valid()
    {
        // Arrange
        var course = CreateCourseWithModule(out var module);
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new UpdateModuleRequest
        {
            CourseId = course.Id,
            ModuleId = module.Id,
            Title = _faker.Commerce.ProductName(),
            Order = _faker.Random.Int(0, 10)
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _courseRepository.Received(1).UpdateAsync(course, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        course.Modules.First().Title.ShouldBe(request.Title);
        course.Modules.First().Order.ShouldBe(request.Order);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_NotFound_When_Course_Not_Found()
    {
        // Arrange
        _courseRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        var request = new UpdateModuleRequest
        {
            CourseId = Guid.NewGuid(), ModuleId = Guid.NewGuid(), Title = _faker.Commerce.ProductName(), Order = 0
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    private Course CreateCourseWithModule(out Module module)
    {
        var course = new CourseTestFixture().CreateValidCourse();
        module = Module.Create(course.Id, _faker.Commerce.ProductName(), 0);
        course.AddModule(module);
        return course;
    }
}