using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.Domain.Exceptions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses;

[Trait("UnitTests", "Domain - Aggregates")]
public sealed class CourseTest(CourseTestFixture fixture) : IClassFixture<CourseTestFixture>
{
    [Fact(DisplayName = nameof(Create_Should_Return_Valid_Course))]
    public void Create_Should_Return_Valid_Course()
    {
        // Arrange
        var instructorId = Guid.NewGuid();
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
        course.InstructorId.ShouldBe(instructorId);
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
        var instructorId = Guid.NewGuid();
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
        var instructorId = Guid.NewGuid();
        var title = fixture.Faker.Commerce.ProductName();

        // Act
        var course = Course.CreateAsDraft(instructorId, title);

        // Assert
        course.ShouldNotBeNull();
        course.InstructorId.ShouldBe(instructorId);
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
        var instructorId = Guid.NewGuid();
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
        var newLanguage = fixture.Faker.Random.ArrayElement(new[] { "French", "German", "Italian" });
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
        var moduleOrder = 1;

        // Act
        course.AddModule(moduleTitle, moduleOrder);

        // Assert
        course.Modules.Count.ShouldBe(1);
        var module = course.Modules.First();
        module.Title.ShouldBe(moduleTitle);
        module.Order.ShouldBe(moduleOrder);
        module.CourseId.ShouldBe(course.Id);
    }

    [Theory(DisplayName = nameof(AddModule_Should_Throw_Exception_For_Restricted_Status))]
    [InlineData(CourseStatus.InReview)]
    [InlineData(CourseStatus.Deleted)]
    public void AddModule_Should_Throw_Exception_For_Restricted_Status(CourseStatus status)
    {
        // Arrange
        var course = fixture.CreateCourseWithStatus(status);
        var moduleTitle = fixture.Faker.Commerce.ProductName();
        var moduleOrder = 1;

        // Act & Assert
        Should.Throw<DomainException>(() =>
            course.AddModule(moduleTitle, moduleOrder)
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
}
