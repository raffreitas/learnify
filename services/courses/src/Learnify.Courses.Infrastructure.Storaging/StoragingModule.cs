using Learnify.Courses.Application.Abstractions.Storage;
using Learnify.Courses.Infrastructure.Storaging.Services;
using Learnify.Courses.Infrastructure.Storaging.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Minio;

using OpenLibs.Extensions;

namespace Learnify.Courses.Infrastructure.Storaging;

public static class StoragingModule
{
    public static IServiceCollection AddStoragingModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings = services.ConfigureRequiredSettings<StorageSettings>(configuration, StorageSettings.SectionName);
        services.AddSingleton<IMinioClient>(_ => new MinioClient()
            .WithEndpoint(settings.Endpoint)
            .WithCredentials(settings.AccessKey, settings.SecretKey)
            .WithSSL(settings.UseSsl)
            .Build());

        services.AddScoped<IStorageService, MinioStorageService>();
        return services;
    }
}