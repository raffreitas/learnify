using Learnify.VideoProcessing.Application.Abstractions;
using Learnify.VideoProcessing.Infrastructure.Storaging.Settings;

using Microsoft.Extensions.Options;

using Minio;
using Minio.DataModel.Args;

namespace Learnify.VideoProcessing.Infrastructure.Storaging.Services;

internal sealed class MinioStorageService(IMinioClient minioClient, IOptions<StorageSettings> options) : IStorageService
{
    private readonly StorageSettings _settings = options.Value;

    public async Task<string> GetPresignedUploadUrlAsync(
        string name,
        TimeSpan? expirationTime = null,
        CancellationToken cancellationToken = default
    )
    {
        await CreateBucketIfNotExistsAsync(cancellationToken);

        var expiration = expirationTime ?? TimeSpan.FromMinutes(15);

        var args = new PresignedPutObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(name)
            .WithExpiry((int)expiration.TotalSeconds);

        return await minioClient.PresignedPutObjectAsync(args);
    }

    private async Task CreateBucketIfNotExistsAsync(CancellationToken cancellationToken = default)
    {
        var bucketName = _settings.BucketName;
        bool exists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName),
            cancellationToken
        );

        if (!exists)
            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), cancellationToken);
    }
}