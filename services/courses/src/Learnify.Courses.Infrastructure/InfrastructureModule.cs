using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Domain.Aggregates.Categories.Repositories;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;
using Learnify.Courses.Infrastructure.Persistence;
using Learnify.Courses.Infrastructure.Persistence.Context;
using Learnify.Courses.Infrastructure.Persistence.Repositories;
using Learnify.Courses.Infrastructure.Persistence.Settings;
using Learnify.Courses.Infrastructure.Persistence.Shared;
using Learnify.Courses.Infrastructure.Shared.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learnify.Courses.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddPersistence(configuration);
        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.GetAndConfigureSettings<DatabaseSettings>(configuration, DatabaseSettings.SectionName);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(settings.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.EnableRetryOnFailure();
            }).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}