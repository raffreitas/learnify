using Learnify.VideoProcessing.Application.Abstractions;
using Learnify.VideoProcessing.Infrastructure.Storaging.Services;
using Learnify.VideoProcessing.Infrastructure.Storaging.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Minio;

using OpenLibs.Extensions;

namespace Learnify.VideoProcessing.Infrastructure.Storaging;

public static class StorageModule
{
    public static IServiceCollection AddStorageModule(
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