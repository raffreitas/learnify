using Learnify.VideoProcessing.Application.Abstractions;
using Learnify.VideoProcessing.Application.Videos.Commands.CreateVideo;
using Learnify.VideoProcessing.Domain.Aggregates.Videos;
using Learnify.VideoProcessing.Domain.Aggregates.Videos.Repositories;

using NSubstitute;

using Shouldly;

namespace Learnify.VideoProcessing.UnitTests.Application.Videos.Commands.CreateVideo;

public sealed class CreateVideoCommandHandlerTests
{
    private readonly IVideoRepository _videoRepository = Substitute.For<IVideoRepository>();
    private readonly IStorageService _storageService = Substitute.For<IStorageService>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly CreateVideoCommandHandler _handler;

    public CreateVideoCommandHandlerTests()
    {
        _handler = new CreateVideoCommandHandler(_videoRepository, _storageService, _unitOfWork);
    }

    [Fact(DisplayName = nameof(CreateVideoCommand))]
    public async Task HandleAsync_ShouldReturnFailure_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new CreateVideoCommand { Filename = string.Empty };

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsFailed.ShouldBeTrue();
        await _videoRepository.DidNotReceive()
            .AddAsync(Arg.Any<Video>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = nameof(HandleAsync_ShouldCreateVideo_WhenCommandIsValid))]
    public async Task HandleAsync_ShouldCreateVideo_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateVideoCommand { Filename = "video.mp4" };
        _storageService
            .GetPresignedUploadUrlAsync(command.Filename, Arg.Any<TimeSpan?>())
            .Returns("https://example.com/presigned-url");

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.VideoId.ShouldNotBe(Guid.Empty);
        result.Value.UploadUrl.ShouldBe("https://example.com/presigned-url");
        await _videoRepository.Received(1)
            .AddAsync(Arg.Any<Video>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}