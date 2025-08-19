using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.Domain.Exceptions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Entities;

[Trait("UnitTests", "Domain - Entities")]
public sealed class LessonTests
{
    [Fact(DisplayName = nameof(Create_Should_Return_Valid_Lesson))]
    public void Create_Should_Return_Valid_Lesson()
    {
        var moduleId = Guid.NewGuid();
        var lessonMedia = LessonMedia.Create(MediaAssetId.Create(Guid.NewGuid()));
        var info = new LessonInfo("Title", "Desc", lessonMedia, 0, true);

        var lesson = Lesson.Create(moduleId, info);

        lesson.ModuleId.ShouldBe(moduleId);
        lesson.Title.ShouldBe(info.Title);
        lesson.Description.ShouldBe(info.Description);
        lesson.Media.Id.ShouldBe(info.Media.Id);
        lesson.Order.ShouldBe(info.Order);
        lesson.IsPublic.ShouldBe(info.IsPublic);
    }

    [Fact(DisplayName = nameof(Create_Should_Throw_When_Invalid_Info))]
    public void Create_Should_Throw_When_Invalid_Info()
    {
        var moduleId = Guid.NewGuid();
        var info = new LessonInfo("", "", null!, -1, false);
        Should.Throw<DomainException>(() => Lesson.Create(moduleId, info));
    }
}