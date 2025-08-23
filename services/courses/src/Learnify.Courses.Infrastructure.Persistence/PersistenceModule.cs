using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Application.Abstractions.Persistence;
using Learnify.Courses.Application.Courses.Abstractions;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.Infrastructure.Persistence.Context;
using Learnify.Courses.Infrastructure.Persistence.Interceptors;
using Learnify.Courses.Infrastructure.Persistence.Queries;
using Learnify.Courses.Infrastructure.Persistence.Repositories;
using Learnify.Courses.Infrastructure.Persistence.Services;
using Learnify.Courses.Infrastructure.Persistence.Settings;
using Learnify.Courses.Infrastructure.Persistence.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OpenLibs.Extensions;

namespace Learnify.Courses.Infrastructure.Persistence;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistenceModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = services.ConfigureRequiredSettings<DatabaseSettings>(
            configuration, DatabaseSettings.SectionName
        );

        services.AddScoped<SavingChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(settings.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.EnableRetryOnFailure();
            });

            options
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<SavingChangesInterceptor>());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IEventOutboxService, EventOutboxService>();

        services.AddScoped<ICourseQueries, CourseQueries>();

        return services;
    }
}