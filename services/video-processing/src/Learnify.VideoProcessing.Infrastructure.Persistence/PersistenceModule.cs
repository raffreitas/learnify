using Learnify.VideoProcessing.Application.Abstractions;
using Learnify.VideoProcessing.Domain.Aggregates.Videos.Repositories;
using Learnify.VideoProcessing.Infrastructure.Persistence.Context;
using Learnify.VideoProcessing.Infrastructure.Persistence.Repositories;
using Learnify.VideoProcessing.Infrastructure.Persistence.Settings;
using Learnify.VideoProcessing.Infrastructure.Persistence.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OpenLibs.Extensions;

namespace Learnify.VideoProcessing.Infrastructure.Persistence;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistenceModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings = services.ConfigureRequiredSettings<DatabaseSettings>(
            configuration,
            DatabaseSettings.SectionName
        );

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(settings.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.EnableRetryOnFailure();
            }).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVideoRepository, VideoRepository>();

        return services;
    }
}