using Grpc.Net.Client;

using Learnify.Courses.Application.Abstractions.VideoProcessing;
using Learnify.Courses.Infrastructure.Integrations.VideoProcessing;
using Learnify.Courses.Infrastructure.Integrations.VideoProcessing.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OpenLibs.Extensions;

namespace Learnify.Courses.Infrastructure.Integrations;

public static class IntegrationsModule
{
    public static IServiceCollection AddIntegrationsModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddVideoProcessingService(configuration);
        return services;
    }

    private static void AddVideoProcessingService(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.ConfigureRequiredSettings<VideoProcessingSettings>(
            configuration, VideoProcessingSettings.SectionName
        );
        services.AddSingleton<VideoService.VideoServiceClient>(sp =>
        {
            var channel = GrpcChannel.ForAddress(settings.BaseUrl);
            return new VideoService.VideoServiceClient(channel);
        });

        services.AddSingleton<UploadManager.UploadManagerClient>(sp =>
        {
            var channel = GrpcChannel.ForAddress(settings.BaseUrl);
            return new UploadManager.UploadManagerClient(channel);
        });

        services.AddScoped<IVideoProcessingService, VideoProcessingService>();
    }
}