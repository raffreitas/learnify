using Learnify.VideoProcessing.Application.Videos.Commands.CreateVideo;
using Learnify.VideoProcessing.Application.Videos.Services;
using Learnify.VideoProcessing.Application.Videos.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace Learnify.VideoProcessing.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddVideosModule();

        return services;
    }

    private static void AddVideosModule(this IServiceCollection services)
    {
        services.AddScoped<IUploadService, UploadService>();
        services.AddScoped<ICreateVideoCommandHandler, CreateVideoCommandHandler>();
    }
}