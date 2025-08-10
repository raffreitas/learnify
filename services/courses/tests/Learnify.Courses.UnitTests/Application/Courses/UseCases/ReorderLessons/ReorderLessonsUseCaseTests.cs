using Bogus;

using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.UseCases.ReorderLessons;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using NSubstitute;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.ReorderLessons;

[Trait("UnitTests", "Application - UseCases")]
public class ReorderLessonsUseCaseTests : IClassFixture<CourseTestFixture>
{
    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly ReorderLessonsUseCase _useCase;
    private readonly CourseTestFixture _fixture;

    public ReorderLessonsUseCaseTests(CourseTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = new ReorderLessonsUseCase(_courseRepository, _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Reorder_Lessons_When_Valid()
    {
        // Arrange
        var course = _fixture.CreateValidCourseWithModule();
        var module = course.Modules.First();
        course.AddLessonToModule(module.Id, _fixture.CreateLessonInfo());
        course.AddLessonToModule(module.Id, _fixture.CreateLessonInfo());
        course.AddLessonToModule(module.Id, _fixture.CreateLessonInfo());
        var l1 = module.Lessons.ElementAt(0);
        var l2 = module.Lessons.ElementAt(1);
        var l3 = module.Lessons.ElementAt(2);

        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);

        var req = new ReorderLessonsRequest
        {
            CourseId = course.Id,
            ModuleId = module.Id,
            Positions = new Dictionary<Guid, int> { [l1.Id] = 2, [l2.Id] = 0, [l3.Id] = 1 }
        };

        // Act
        var result = await _useCase.ExecuteAsync(req);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _courseRepository.Received(1).UpdateAsync(course, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        course.Modules.First().Lessons.Select(l => l.Id).ShouldBe([l2.Id, l3.Id, l1.Id]);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Fail_When_Positions_Not_Matching_Lessons()
    {
        // Arrange
        var course = _fixture.CreateValidCourseWithModule();
        var module = course.Modules.First();
        course.AddLessonToModule(module.Id, _fixture.CreateLessonInfo());
        course.AddLessonToModule(module.Id, _fixture.CreateLessonInfo());
        var l1 = module.Lessons.ElementAt(0);
        _courseRepository.GetByIdAsync(course.Id, Arg.Any<CancellationToken>()).Returns(course);
        var request = new ReorderLessonsRequest
        {
            CourseId = course.Id, ModuleId = module.Id, Positions = new Dictionary<Guid, int> { [l1.Id] = 1 }
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}