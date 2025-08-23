using Grpc.Core;

using Learnify.VideoProcessing.Application.Videos.Services.Abstractions;
using Learnify.VideoProcessing.Application.Videos.Services.DTOs;

namespace Learnify.VideoProcessing.gRPC.Services;

public sealed class UploadManagerService(IUploadService uploadService) : UploadManager.UploadManagerBase
{
    public override async Task<GetPresignedURLResponse> GetPresignedURL(
        GetPresignedURLRequest request,
        ServerCallContext context
    )
    {
        var response = await uploadService.GetPresignedUploadUrlAsync(
            new GetPresignedUploadUrlRequest(
                request.FileName,
                TimeSpan.FromSeconds(request.Expiration)
            ));

        if (response.IsFailed)
            throw new RpcException(new Status(StatusCode.Internal, response.Errors[0].Message));

        return new GetPresignedURLResponse
        {
            Expiration = (int)response.Value.Expiration.TotalSeconds, Url = response.Value.Url
        };
    }
}