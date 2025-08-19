using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Entities;
using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.Models;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.UnitTests.Shared.Fixtures;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses;

[Trait("UnitTests", "Domain - Aggregates")]
public sealed class CourseTests(CourseTestFixture fixture) : IClassFixture<CourseTestFixture>
{
    [Fact(DisplayName = nameof(Create_Should_Return_Valid_Course))]
    public void Create_Should_Return_Valid_Course()
    {
        // Arrange
        var instructorId = InstructorId.Create(Guid.NewGuid());
        var title = fixture.Faker.Commerce.ProductName();
        var description = fixture.Faker.Commerce.ProductDescription();
        var imageUrl = fixture.Faker.Internet.Url();
        var price = Price.Create(fixture.Faker.Random.Decimal(10, 100));
        var language = fixture.Faker.Random.ArrayElement(["English", "Spanish", "Portuguese"]);
        var difficultyLevel = fixture.Faker.PickRandom<DifficultyLevel>();
        var status = CourseStatus.Draft;

        // Act
        var course = Course.Create(
            instructorId,
            title,
            description,
            imageUrl,
            price,
            language,
            difficultyLevel,
            status);

        // Assert
        course.ShouldNotBeNull();
        course.Instructor.ShouldBe(instructorId);
        course.Title.ShouldBe(title);
        course.Description.ShouldBe(description);
        course.ImageUrl.ShouldBe(imageUrl);
        course.Price.ShouldBe(price);
        course.Language.ShouldBe(language);
        course.DifficultyLevel.ShouldBe(difficultyLevel);
        course.Status.ShouldBe(status);
    }

    [Fact(DisplayName = nameof(Create_Should_Throw_Exception_When_Missing_Required_Fields))]
    public void Create_Should_Throw_Exception_When_Missing_Required_Fields()
    {
        // Arrange
        var instructorId = InstructorId.Create(Guid.NewGuid());
        var title = "";
        var description = "";
        var imageUrl = "";
        var price = Price.Create(0);
        var language = "";
        var difficultyLevel = DifficultyLevel.Beginner;
        var status = CourseStatus.Draft;

        // Act & Assert
        Should.Throw<DomainException>(() =>
            Course.Create(
                instructorId,
                title,
                description,
                imageUrl,
                price,
                language,
                difficultyLevel,
                status
            )
        ).Message.ShouldBe("Course must have basic information.");
    }

    [Fact(DisplayName = nameof(CreateAsDraft_Should_Return_Valid_Draft_Course))]
    public void CreateAsDraft_Should_Return_Valid_Draft_Course()
    {
        // Arrange
        var instructorId = InstructorId.Create(Guid.NewGuid());
        var title = fixture.Faker.Commerce.ProductName();

        // Act
        var course = Course.CreateAsDraft(instructorId, title);

        // Assert
        course.ShouldNotBeNull();
        course.Instructor.ShouldBe(instructorId);
        course.Title.ShouldBe(title);
        course.Status.ShouldBe(CourseStatus.Draft);
        course.Description.ShouldBe(string.Empty);
        course.ImageUrl.ShouldBe(string.Empty);
        course.Language.ShouldBe(string.Empty);
        course.DifficultyLevel.ShouldBe(DifficultyLevel.Beginner);
    }

    [Fact(DisplayName = nameof(CreateAsDraft_Should_Throw_Exception_When_Title_Is_Missing))]
    public void CreateAsDraft_Should_Throw_Exception_When_Title_Is_Missing()
    {
        // Arrange
        var instructorId = InstructorId.Create(Guid.NewGuid());
        var title = "";

        // Act & Assert
        Should.Throw<DomainException>(() =>
            Course.CreateAsDraft(instructorId, title)
        ).Message.ShouldBe("Course cannot be created as draft without content.");
    }

    [Fact(DisplayName = nameof(UpdateCourseInfo_Should_Update_Course_Properties))]
    public void UpdateCourseInfo_Should_Update_Course_Properties()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        var newDescription = fixture.Faker.Commerce.ProductDescription();
        var newImageUrl = fixture.Faker.Internet.Url();
        var newPrice = Price.Create(fixture.Faker.Random.Decimal(10, 100));
        var newLanguage = fixture.Faker.Random.ArrayElement(["French", "German", "Italian"]);
        var newDifficultyLevel = DifficultyLevel.Advanced;

        // Act
        course.UpdateCourseInfo(newDescription, newImageUrl, newPrice, newLanguage, newDifficultyLevel);

        // Assert
        course.Description.ShouldBe(newDescription);
        course.ImageUrl.ShouldBe(newImageUrl);
        course.Price.ShouldBe(newPrice);
        course.Language.ShouldBe(newLanguage);
        course.DifficultyLevel.ShouldBe(newDifficultyLevel);
    }

    [Theory(DisplayName = nameof(UpdateCourseInfo_Should_Throw_Exception_For_Restricted_Status))]
    [InlineData(CourseStatus.InReview)]
    [InlineData(CourseStatus.Deleted)]
    public void UpdateCourseInfo_Should_Throw_Exception_For_Restricted_Status(CourseStatus status)
    {
        // Arrange
        var course = fixture.CreateCourseWithStatus(status);
        var newDescription = fixture.Faker.Commerce.ProductDescription();
        var newImageUrl = fixture.Faker.Internet.Url();
        var newPrice = Price.Create(fixture.Faker.Random.Decimal(10, 100));
        var newLanguage = fixture.Faker.Random.ArrayElement(["French", "German", "Italian"]);
        var newDifficultyLevel = DifficultyLevel.Advanced;

        // Act & Assert
        Should.Throw<DomainException>(() =>
            course.UpdateCourseInfo(newDescription, newImageUrl, newPrice, newLanguage, newDifficultyLevel)
        ).Message.ShouldBe("Unable to update information for this course");
    }

    [Fact(DisplayName = nameof(AddModule_Should_Add_Module_To_Course))]
    public void AddModule_Should_Add_Module_To_Course()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        var moduleTitle = fixture.Faker.Commerce.ProductName();
        const int moduleOrder = 1;
        var module = Module.Create(course.Id, moduleTitle, moduleOrder);

        // Act
        course.AddModule(module);

        // Assert
        course.Modules.Count.ShouldBe(1);
        var addedModule = course.Modules.First();
        addedModule.Title.ShouldBe(moduleTitle);
        addedModule.Order.ShouldBe(moduleOrder);
        addedModule.CourseId.ShouldBe(course.Id);
    }

    [Theory(DisplayName = nameof(AddModule_Should_Throw_Exception_For_Restricted_Status))]
    [InlineData(CourseStatus.InReview)]
    [InlineData(CourseStatus.Deleted)]
    public void AddModule_Should_Throw_Exception_For_Restricted_Status(CourseStatus status)
    {
        // Arrange
        var course = fixture.CreateCourseWithStatus(status);
        var moduleTitle = fixture.Faker.Commerce.ProductName();
        const int moduleOrder = 1;
        var module = Module.Create(course.Id, moduleTitle, moduleOrder);

        // Act & Assert
        Should.Throw<DomainException>(() =>
            course.AddModule(module)
        ).Message.ShouldBe("Unable to add module for this course");
    }

    [Fact(DisplayName = nameof(AddLessonToModule_Should_Add_Lesson_To_The_Module))]
    public void AddLessonToModule_Should_Add_Lesson_To_The_Module()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModule();
        var moduleId = course.Modules.First().Id;
        var lessonInfo = fixture.CreateLessonInfo();

        // Act
        course.AddLessonToModule(moduleId, lessonInfo);

        // Assert
        var module = course.Modules.First();
        module.Lessons.Count.ShouldBe(1);
        var lesson = module.Lessons.First();
        lesson.Title.ShouldBe(lessonInfo.Title);
        lesson.Description.ShouldBe(lessonInfo.Description);
        lesson.VideoUrl.ShouldBe(lessonInfo.VideoUrl);
        lesson.Order.ShouldBe(lessonInfo.Order);
        lesson.IsPublic.ShouldBe(lessonInfo.IsPublic);
    }

    [Fact(DisplayName = nameof(AddLessonToModule_Should_Throw_Exception_For_Invalid_Module))]
    public void AddLessonToModule_Should_Throw_Exception_For_Invalid_Module()
    {
        // Arrange
        var course = fixture.CreateValidCourse();
        var invalidModuleId = Guid.NewGuid();
        var lessonInfo = fixture.CreateLessonInfo();

        // Act & Assert
        Should.Throw<DomainException>(() =>
            course.AddLessonToModule(invalidModuleId, lessonInfo)
        ).Message.ShouldBe("Module not found.");
    }

    [Fact(DisplayName = nameof(RequestReview_Should_Change_Status_To_InReview))]
    public void RequestReview_Should_Change_Status_To_InReview()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModuleAndCategoryAndLesson();

        // Act
        course.RequestReview();

        // Assert
        course.Status.ShouldBe(CourseStatus.InReview);
    }

    [Fact(DisplayName = nameof(RequestReview_Should_Throw_Exception_When_No_Content))]
    public void RequestReview_Should_Throw_Exception_When_No_Content()
    {
        // Arrange
        var course = fixture.CreateValidCourse();

        // Act & Assert
        Should.Throw<DomainException>(() =>
            course.RequestReview()
        ).Message.ShouldBe("Course cannot be reviewed without content.");
    }

    [Fact(DisplayName = nameof(Publish_Should_Change_Status_To_Published))]
    public void Publish_Should_Change_Status_To_Published()
    {
        // Arrange
        var course = fixture.CreateValidCourseWithModuleAndCategoryAndLesson();
        course.RequestReview();
        course.ApproveForPublish();
        // Act
        course.Publish();

        // Assert
        course.Status.ShouldBe(CourseStatus.Published);
    }

    [Fact(DisplayName = nameof(Publish_Should_Throw_Exception_When_No_Content))]
    public void Publish_Should_Throw_Exception_When_No_Content()
    {
        // Arrange
        var course = fixture.CreateValidCourse();

        // Act & Assert
        Should.Throw<DomainException>(course.Publish).Message.ShouldBe("Course cannot be published without content.");
    }

    [Fact(DisplayName = nameof(UpdateModule_Should_Update_Info_When_Valid))]
    public void UpdateModule_Should_Update_Info_When_Valid()
    {
        var course = fixture.CreateValidCourseWithModule();
        var module = course.Modules.First();

        course.UpdateModule(module.Id, "New Title", 5);

        var updated = course.Modules.First();
        updated.Title.ShouldBe("New Title");
        updated.Order.ShouldBe(5);
    }

    [Fact(DisplayName = nameof(UpdateModule_Should_Throw_When_Module_Not_Found))]
    public void UpdateModule_Should_Throw_When_Module_Not_Found()
    {
        var course = fixture.CreateValidCourseWithModule();
        Should.Throw<DomainException>(() => course.UpdateModule(Guid.NewGuid(), "T", 0))
            .Message.ShouldBe("Module not found.");
    }

    [Theory(DisplayName = nameof(UpdateModule_Should_Throw_When_Restricted_Status))]
    [InlineData(CourseStatus.InReview)]
    [InlineData(CourseStatus.Deleted)]
    public void UpdateModule_Should_Throw_When_Restricted_Status(CourseStatus status)
    {
        var course = fixture.CreateCourseWithStatus(status);
        Should.Throw<DomainException>(() => course.UpdateModule(Guid.NewGuid(), "T", 0))
            .Message.ShouldBe("Unable to update module for this course.");
    }

    [Fact(DisplayName = nameof(UpdateLesson_Should_Update_When_Valid))]
    public void UpdateLesson_Should_Update_When_Valid()
    {
        var course = fixture.CreateValidCourseWithModule();
        var module = course.Modules.First();
        course.AddLessonToModule(module.Id, fixture.CreateLessonInfo());
        var lesson = module.Lessons.First();

        var info = new LessonInfo("T", "D", "U", 3, true);
        course.UpdateLesson(module.Id, lesson.Id, info);

        var updated = module.Lessons.First();
        updated.Title.ShouldBe("T");
        updated.Order.ShouldBe(3);
        updated.IsPublic.ShouldBeTrue();
    }

    [Fact(DisplayName = nameof(UpdateLesson_Should_Throw_When_Module_Not_Found))]
    public void UpdateLesson_Should_Throw_When_Module_Not_Found()
    {
        var course = fixture.CreateValidCourseWithModule();
        var info = new LessonInfo("T", "D", "U", 3, true);
        Should.Throw<DomainException>(() => course.UpdateLesson(Guid.NewGuid(), Guid.NewGuid(), info))
            .Message.ShouldBe("Module not found.");
    }

    [Fact(DisplayName = nameof(UpdateLesson_Should_Throw_When_Lesson_Not_Found))]
    public void UpdateLesson_Should_Throw_When_Lesson_Not_Found()
    {
        var course = fixture.CreateValidCourseWithModule();
        var module = course.Modules.First();
        var info = new LessonInfo("T", "D", "U", 3, true);
        Should.Throw<DomainException>(() => course.UpdateLesson(module.Id, Guid.NewGuid(), info))
            .Message.ShouldBe("Lesson not found.");
    }

    [Theory(DisplayName = nameof(UpdateLesson_Should_Throw_When_Restricted_Status))]
    [InlineData(CourseStatus.InReview)]
    [InlineData(CourseStatus.Deleted)]
    public void UpdateLesson_Should_Throw_When_Restricted_Status(CourseStatus status)
    {
        var course = fixture.CreateCourseWithStatus(status);
        Should.Throw<DomainException>(() =>
                course.UpdateLesson(Guid.NewGuid(), Guid.NewGuid(), new LessonInfo("T", "D", "U", 0, false)))
            .Message.ShouldBe("Unable to update lesson for this course.");
    }

    [Fact(DisplayName = nameof(ReorderModules_Should_Reorder_By_Positions))]
    public void ReorderModules_Should_Reorder_By_Positions()
    {
        var course = fixture.CreateValidCourse();
        var m1 = Module.Create(course.Id, fixture.Faker.Commerce.ProductName(), 0);
        var m2 = Module.Create(course.Id, fixture.Faker.Commerce.ProductName(), 1);
        var m3 = Module.Create(course.Id, fixture.Faker.Commerce.ProductName(), 2);
        course.AddModule(m1);
        course.AddModule(m2);
        course.AddModule(m3);

        var positions = new Dictionary<Guid, int> { [m1.Id] = 2, [m2.Id] = 0, [m3.Id] = 1 };

        course.ReorderModules(positions);

        course.Modules.Select(m => m.Id).ShouldBe([m2.Id, m3.Id, m1.Id]);
        course.Modules.ElementAt(0).Order.ShouldBe(0);
        course.Modules.ElementAt(1).Order.ShouldBe(1);
        course.Modules.ElementAt(2).Order.ShouldBe(2);
    }

    [Theory(DisplayName = nameof(ReorderModules_Should_Throw_When_Restricted_Status))]
    [InlineData(CourseStatus.InReview)]
    [InlineData(CourseStatus.Deleted)]
    public void ReorderModules_Should_Throw_When_Restricted_Status(CourseStatus status)
    {
        var course = fixture.CreateCourseWithStatus(status);
        Should.Throw<DomainException>(() => course.ReorderModules(new()))
            .Message.ShouldBe("Unable to reorder modules for this course.");
    }

    [Fact(DisplayName = nameof(ReorderLessons_Should_Reorder_By_Positions))]
    public void ReorderLessons_Should_Reorder_By_Positions()
    {
        var course = fixture.CreateValidCourseWithModule();
        var module = course.Modules.First();
        course.AddLessonToModule(module.Id, fixture.CreateLessonInfo());
        course.AddLessonToModule(module.Id, fixture.CreateLessonInfo());
        course.AddLessonToModule(module.Id, fixture.CreateLessonInfo());
        var l1 = module.Lessons.ElementAt(0);
        var l2 = module.Lessons.ElementAt(1);
        var l3 = module.Lessons.ElementAt(2);

        course.ReorderLessons(module.Id, new Dictionary<Guid, int> { [l1.Id] = 2, [l2.Id] = 0, [l3.Id] = 1 });

        module.Lessons.Select(l => l.Id).ShouldBe([l2.Id, l3.Id, l1.Id]);
    }

    [Fact(DisplayName = nameof(ReorderLessons_Should_Throw_When_Module_Not_Found))]
    public void ReorderLessons_Should_Throw_When_Module_Not_Found()
    {
        var course = fixture.CreateValidCourseWithModule();
        Should.Throw<DomainException>(() => course.ReorderLessons(Guid.NewGuid(), new()))
            .Message.ShouldBe("Module not found.");
    }

    [Theory(DisplayName = nameof(ReorderLessons_Should_Throw_When_Restricted_Status))]
    [InlineData(CourseStatus.InReview)]
    [InlineData(CourseStatus.Deleted)]
    public void ReorderLessons_Should_Throw_When_Restricted_Status(CourseStatus status)
    {
        var course = fixture.CreateCourseWithStatus(status);
        Should.Throw<DomainException>(() => course.ReorderLessons(Guid.NewGuid(), new()))
            .Message.ShouldBe("Unable to reorder lessons for this course.");
    }

    [Fact(DisplayName = nameof(AddModule_Should_Throw_When_Duplicate_Title))]
    public void AddModule_Should_Throw_When_Duplicate_Title()
    {
        var course = fixture.CreateValidCourse();
        var title = "Same";
        course.AddModule(Module.Create(course.Id, title, 0));

        Should.Throw<DomainException>(() => course.AddModule(Module.Create(course.Id, title, 1)))
            .Message.ShouldBe("Module with the same title already exists.");
    }

    [Fact(DisplayName = nameof(ModuleExists_Should_Return_True_When_Title_Duplicate_Ignoring_Case))]
    public void ModuleExists_Should_Return_True_When_Title_Duplicate_Ignoring_Case()
    {
        var course = fixture.CreateValidCourse();
        course.AddModule(Module.Create(course.Id, "Intro", 0));
        var another = Module.Create(course.Id, "intro", 1);
        course.ModuleExists(another).ShouldBeTrue();
    }
}