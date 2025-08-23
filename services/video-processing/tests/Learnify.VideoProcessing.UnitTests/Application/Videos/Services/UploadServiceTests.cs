using Learnify.VideoProcessing.Application.Abstractions;
using Learnify.VideoProcessing.Application.Videos.Services;
using Learnify.VideoProcessing.Application.Videos.Services.DTOs;

using NSubstitute;

using Shouldly;

namespace Learnify.VideoProcessing.UnitTests.Application.Videos.Services;

public sealed class UploadServiceTests
{
    private readonly IStorageService _storageService = Substitute.For<IStorageService>();
    private readonly UploadService _uploadService;

    public UploadServiceTests()
    {
        _uploadService = new UploadService(_storageService);
    }

    [Fact(DisplayName = nameof(GetPresignedUploadUrlAsync_ShouldReturnUrl_WhenRequestIsValid))]
    public async Task GetPresignedUploadUrlAsync_ShouldReturnUrl_WhenRequestIsValid()
    {
        // Arrange
        var request = new GetPresignedUploadUrlRequest("video.mp4", TimeSpan.FromMinutes(10));
        const string expectedUrl = "https://example.com/presigned-url";

        _storageService
            .GetPresignedUploadUrlAsync(request.Name, request.ExpirationTime, Arg.Any<CancellationToken>())
            .Returns(expectedUrl);

        // Act
        var result = await _uploadService.GetPresignedUploadUrlAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Url.ShouldBe(expectedUrl);
        result.Value.Expiration.ShouldBe(request.ExpirationTime ?? TimeSpan.FromMinutes(15));
    }

    [Fact(DisplayName = nameof(GetPresignedUploadUrlAsync_ShouldReturnError_WhenExpirationTimeIsInvalid))]
    public async Task GetPresignedUploadUrlAsync_ShouldReturnError_WhenExpirationTimeIsInvalid()
    {
        // Arrange
        var request = new GetPresignedUploadUrlRequest("video.mp4", TimeSpan.FromHours(13));

        // Act
        var result = await _uploadService.GetPresignedUploadUrlAsync(request);

        // Assert
        result.IsFailed.ShouldBeTrue();
        result.Errors.ShouldContain(e => e.Message == "Expiration time must be between 1 second and 12 hours.");
    }
}