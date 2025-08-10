using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
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
        var info = new LessonInfo("Title", "Desc", "https://video", 0, true);

        var lesson = Lesson.Create(moduleId, info);

        lesson.ModuleId.ShouldBe(moduleId);
        lesson.Title.ShouldBe(info.Title);
        lesson.Description.ShouldBe(info.Description);
        lesson.VideoUrl.ShouldBe(info.VideoUrl);
        lesson.Order.ShouldBe(info.Order);
        lesson.IsPublic.ShouldBe(info.IsPublic);
    }

    [Fact(DisplayName = nameof(Create_Should_Throw_When_Invalid_Info))]
    public void Create_Should_Throw_When_Invalid_Info()
    {
        var moduleId = Guid.NewGuid();
        var info = new LessonInfo("", "", "", -1, false);
        Should.Throw<DomainException>(() => Lesson.Create(moduleId, info));
    }

    [Fact(DisplayName = nameof(UpdateInfo_Should_Update_Fields))]
    public void UpdateInfo_Should_Update_Fields()
    {
        var moduleId = Guid.NewGuid();
        var lesson = Lesson.Create(moduleId, new LessonInfo("A", "B", "C", 0, false));
        var info = new LessonInfo("T", "D", "U", 2, true);

        lesson.UpdateInfo(info);

        lesson.Title.ShouldBe("T");
        lesson.Description.ShouldBe("D");
        lesson.VideoUrl.ShouldBe("U");
        lesson.Order.ShouldBe(2);
        lesson.IsPublic.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(UpdateOrder_Should_Update_Order))]
    public void UpdateOrder_Should_Update_Order()
    {
        var lesson = Lesson.Create(Guid.NewGuid(), new LessonInfo("A", "B", "C", 0, false));
        lesson.UpdateOrder(10);
        lesson.Order.ShouldBe(10);
    }

    [Fact(DisplayName = nameof(UpdateOrder_Should_Throw_For_Negative))]
    public void UpdateOrder_Should_Throw_For_Negative()
    {
        var lesson = Lesson.Create(Guid.NewGuid(), new LessonInfo("A", "B", "C", 0, false));
        Should.Throw<DomainException>(() => lesson.UpdateOrder(-1));
    }
}