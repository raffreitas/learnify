using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Courses.UseCases.UploadCourseImage;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

using NSubstitute;

using Shouldly;

namespace Learnify.Courses.UnitTests.Application.Courses.UseCases.UploadCourseImage;

[Trait("UnitTests", "Application - UseCases")]
public class UploadCourseImageUseCaseTests
{
    private readonly IStorageService _storageService = Substitute.For<IStorageService>();
    private readonly ICourseRepository _courseRepository = Substitute.For<ICourseRepository>();
    private readonly UploadCourseImageUseCase _useCase;

    public UploadCourseImageUseCaseTests()
    {
        _useCase = new UploadCourseImageUseCase(_storageService, _courseRepository);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Upload_When_Valid))]
    public async Task ExecuteAsync_Should_Upload_When_Valid()
    {
        // Arrange
        var stream = new MemoryStream(new byte[10]);
        var courseId = Guid.NewGuid();
        _courseRepository.ExistsByIdAsync(courseId, Arg.Any<CancellationToken>()).Returns(true);
        var request =
            new UploadCourseImageRequest { CourseId = courseId, FileStream = stream, ContentType = "image/png" };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.File.ShouldContain($"courses/assets/{courseId}/cover/original");
        await _storageService.Received(1).UploadFileAsync(
            stream,
            Arg.Any<string>(),
            request.ContentType,
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Fail_When_Course_Not_Found))]
    public async Task ExecuteAsync_Should_Fail_When_Course_Not_Found()
    {
        // Arrange
        var request = new UploadCourseImageRequest
        {
            CourseId = Guid.NewGuid(), FileStream = new MemoryStream(new byte[10]), ContentType = "image/png"
        };
        _courseRepository.ExistsByIdAsync(request.CourseId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }

    [Theory(DisplayName = nameof(ExecuteAsync_Should_Validate_Stream_Length))]
    [InlineData(0, true)]
    [InlineData(6 * 1024 * 1024, true)]
    [InlineData(1024, false)]
    public async Task ExecuteAsync_Should_Validate_Stream_Length(long len, bool expectFail)
    {
        // Arrange
        var courseId = Guid.NewGuid();
        _courseRepository.ExistsByIdAsync(courseId, Arg.Any<CancellationToken>()).Returns(true);
        var bytes = new byte[len > 0 ? len : 0];
        var ms = new MemoryStream(bytes);
        var request = new UploadCourseImageRequest { CourseId = courseId, FileStream = ms, ContentType = "image/jpeg" };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        (result.IsFailed).ShouldBe(expectFail);
    }

    [Theory(DisplayName = nameof(ExecuteAsync_Should_Validate_ContentType))]
    [InlineData("image/png", true)]
    [InlineData("image/jpeg", true)]
    [InlineData("image/gif", false)]
    [InlineData("application/pdf", false)]
    public async Task ExecuteAsync_Should_Validate_ContentType(string contentType, bool supported)
    {
        // Arrange
        var courseId = Guid.NewGuid();
        _courseRepository.ExistsByIdAsync(courseId, Arg.Any<CancellationToken>()).Returns(true);
        var stream = new MemoryStream(new byte[10]);
        var request = new UploadCourseImageRequest
        {
            CourseId = courseId, FileStream = stream, ContentType = contentType
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        (result.IsSuccess).ShouldBe(supported);
    }

    [Fact(DisplayName = nameof(ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid))]
    public async Task ExecuteAsync_Should_Return_ValidationError_When_Request_Invalid()
    {
        // Arrange
        var request = new UploadCourseImageRequest
        {
            CourseId = Guid.Empty, FileStream = new MemoryStream(), ContentType = ""
        };

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
    }
}