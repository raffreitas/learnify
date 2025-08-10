using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Application.Abstractions.Storage;
using Learnify.Courses.Infrastructure.Storage.Settings;

using Microsoft.Extensions.Options;

using Minio;
using Minio.DataModel.Args;

namespace Learnify.Courses.Infrastructure.Storage.Services;

internal sealed class MinioStorageService(IMinioClient minioClient, IOptions<StorageSettings> options) : IStorageService
{
    private readonly StorageSettings _settings = options.Value;

    public async Task UploadFileAsync(
        Stream fileStream,
        string name,
        string contentType,
        CancellationToken cancellationToken = default
    )
    {
        await CreateBucketIfNotExistsAsync(_settings.BucketName, cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(name)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);
        await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
    }

    public async Task<string> GetFileUrlAsync(
        string name,
        TimeSpan? expirationTime = null,
        CancellationToken cancellationToken = default
    )
    {
        var expiration = expirationTime.HasValue && expirationTime.Value > TimeSpan.Zero
            ? expirationTime.Value
            : TimeSpan.FromMinutes(_settings.DefaultExpirationInMinutes);

        var args = new PresignedGetObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(name)
            .WithExpiry((int)expiration.TotalSeconds);
        return await minioClient.PresignedGetObjectAsync(args);
    }

    private async Task CreateBucketIfNotExistsAsync(string bucketName, CancellationToken cancellationToken = default)
    {
        bool exists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName),
            cancellationToken
        );

        if (!exists)
            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), cancellationToken);
    }
}