using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Exceptions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.Entities;

[Trait("UnitTests", "Domain - Entities")]
public sealed class ModuleTests
{
    [Fact(DisplayName = nameof(Create_Should_Return_Valid_Module))]
    public void Create_Should_Return_Valid_Module()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var title = "Intro";
        var order = 0;

        // Act
        var module = Module.Create(courseId, title, order);

        // Assert
        module.CourseId.ShouldBe(courseId);
        module.Title.ShouldBe(title);
        module.Order.ShouldBe(order);
    }

    [Theory(DisplayName = nameof(Create_Should_Throw_For_Invalid_Title))]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_Throw_For_Invalid_Title(string invalid)
    {
        // Arrange
        var courseId = Guid.NewGuid();

        // Act & Assert
        Should.Throw<DomainException>(() => Module.Create(courseId, invalid, 0));
    }

    [Fact(DisplayName = nameof(Create_Should_Throw_For_Negative_Order))]
    public void Create_Should_Throw_For_Negative_Order()
    {
        // Arrange
        var courseId = Guid.NewGuid();

        // Act & Assert
        Should.Throw<DomainException>(() => Module.Create(courseId, "Title", -1));
    }

    [Fact(DisplayName = nameof(UpdateInfo_Should_Change_Title_And_Order))]
    public void UpdateInfo_Should_Change_Title_And_Order()
    {
        var module = Module.Create(Guid.NewGuid(), "Old", 0);

        module.UpdateInfo("New", 3);

        module.Title.ShouldBe("New");
        module.Order.ShouldBe(3);
    }

    [Fact(DisplayName = nameof(UpdateOrder_Should_Set_Order))]
    public void UpdateOrder_Should_Set_Order()
    {
        var module = Module.Create(Guid.NewGuid(), "M", 0);
        module.UpdateOrder(5);
        module.Order.ShouldBe(5);
    }

    [Fact(DisplayName = nameof(UpdateOrder_Should_Throw_When_Negative))]
    public void UpdateOrder_Should_Throw_When_Negative()
    {
        var module = Module.Create(Guid.NewGuid(), "M", 0);
        Should.Throw<DomainException>(() => module.UpdateOrder(-1));
    }

    [Fact(DisplayName = nameof(ReorderLessons_Should_Reorder_By_Given_Positions))]
    public void ReorderLessons_Should_Reorder_By_Given_Positions()
    {
        // Arrange
        var module = Module.Create(Guid.NewGuid(), "M", 0);
        var l1 = Lesson.Create(module.Id, new LessonInfo("A", "d", "u", 0, false));
        var l2 = Lesson.Create(module.Id, new LessonInfo("B", "d", "u", 1, false));
        var l3 = Lesson.Create(module.Id, new LessonInfo("C", "d", "u", 2, false));
        // add via reflection since Module adds lessons internally through AddLesson
        var lessonsField = typeof(Module).GetField("_lessons",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        var list = (List<Lesson>)lessonsField.GetValue(module)!;
        list.AddRange([l1, l2, l3]);

        var positions = new Dictionary<Guid, int> { [l1.Id] = 2, [l2.Id] = 0, [l3.Id] = 1 };

        // Act
        module.ReorderLessons(positions);

        // Assert
        module.Lessons.Select(x => x.Id).ShouldBe([l2.Id, l3.Id, l1.Id]);
    }
}