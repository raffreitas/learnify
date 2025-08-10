using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.UseCases.ReorderModules;
using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.ReorderModules;

[Trait("UnitTests", "Application - UseCases")]
public class ReorderModulesUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly ReorderModulesUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public ReorderModulesUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new ReorderModulesUseCase(_courseRepository, _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Reorder_Modules_When_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourse();
        var m1 = Module.Create(course.Id, _fixture.Faker.Commerce.ProductName(), 0);
        var m2 = Module.Create(course.Id, _fixture.Faker.Commerce.ProductName(), 1);
        var m3 = Module.Create(course.Id, _fixture.Faker.Commerce.ProductName(), 2);
        course.AddModule(m1);
        course.AddModule(m2);
        course.AddModule(m3);
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var request = new ReorderModulesRequest
        {
            CourseId = course.Id, Positions = new Dictionary<Guid, int> { [m1.Id] = 2, [m2.Id] = 0, [m3.Id] = 1 }
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _courseRepository.Received(1).UpdateAsync(course, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        course.Modules.Select(m => m.Id).ShouldBe([m2.Id, m3.Id, m1.Id]);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Fail_When_Positions_Not_Matching_Modules()
    {
        // Arrange
        var course = _fixture.CreateValidCourse();
        var m1 = Module.Create(course.Id, _fixture.Faker.Commerce.ProductName(), 0);
        var m2 = Module.Create(course.Id, _fixture.Faker.Commerce.ProductName(), 1);
        course.AddModule(m1);
        course.AddModule(m2);
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);
        var req = new ReorderModulesRequest { CourseId = course.Id, Positions = new() { [m1.Id] = 1 } };

        // Act
        var result = await _useCase.ExecuteAsync(req);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}