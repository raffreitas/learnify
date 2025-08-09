using Learnify.Courses.Application.Abstractions;

using Minio;
using Minio.DataModel.Args;

namespace Learnify.Courses.Infrastructure.Storage.Services;

internal sealed class MinioStorageService(IMinioClient minioClient) : IStorageService
{
    public async Task<string> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string bucketName,
        CancellationToken cancellationToken = default
    )
    {
        await CreateBucketIfNotExistsAsync(bucketName, cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);
        await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        return $"{bucketName}/{fileName}";
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