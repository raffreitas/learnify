using FluentResults;

using Learnify.VideoProcessing.Application.Abstractions;
using Learnify.VideoProcessing.Application.Shared.Extensions;
using Learnify.VideoProcessing.Domain.Aggregates.Videos;
using Learnify.VideoProcessing.Domain.Aggregates.Videos.Repositories;

namespace Learnify.VideoProcessing.Application.Videos.Commands.CreateVideo;

public interface ICreateVideoCommandHandler
{
    Task<Result<CreateVideoCommandResult>> HandleAsync(
        CreateVideoCommand command,
        CancellationToken cancellationToken = default
    );
}

internal sealed class CreateVideoCommandHandler(
    IVideoRepository videoRepository,
    IStorageService storageService,
    IUnitOfWork unitOfWork
) : ICreateVideoCommandHandler
{
    public async Task<Result<CreateVideoCommandResult>> HandleAsync(
        CreateVideoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = command.Validate();
        if (!validationResult.IsValid)
            return Result.Fail(validationResult.GetValidationError());

        var video = Video.Create(command.Filename);

        var uploadExpirationTime = TimeSpan.FromMinutes(15);

        var uploadUrl = await storageService.GetPresignedUploadUrlAsync(
            command.Filename,
            TimeSpan.FromMinutes(15),
            cancellationToken
        );

        await videoRepository.AddAsync(video, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateVideoCommandResult(video.Id, uploadUrl, uploadExpirationTime);
    }
}