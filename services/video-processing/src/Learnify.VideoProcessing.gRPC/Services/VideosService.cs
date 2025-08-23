using Grpc.Core;

using Learnify.VideoProcessing.Application.Videos.Commands.CreateVideo;

namespace Learnify.VideoProcessing.gRPC.Services;

public sealed class VideosService(
    ICreateVideoCommandHandler createVideoCommandHandler
) : VideoService.VideoServiceBase
{
    public override async Task<CreateVideoResponse> CreateVideo(CreateVideoRequest request, ServerCallContext context)
    {
        var command = new CreateVideoCommand { Filename = request.FileName };
        var result = await createVideoCommandHandler.HandleAsync(command, context.CancellationToken);
        if (result.IsFailed)
            throw new RpcException(new Status(StatusCode.Internal, result.Errors[0].Message));

        return new CreateVideoResponse
        {
            UploadExpiration = (int)result.Value.Expiration.TotalSeconds,
            UploadUrl = result.Value.UploadUrl,
            VideoId = result.Value.VideoId.ToString()
        };
    }
}