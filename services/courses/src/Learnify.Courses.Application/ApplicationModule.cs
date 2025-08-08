using Learnify.Courses.Application.Categories.UseCases.CreateCategory;
using Learnify.Courses.Application.Courses.UseCases.CreateCourse;
using Learnify.Courses.Application.Courses.UseCases.UpdateCourse;

using Microsoft.Extensions.DependencyInjection;

namespace Learnify.Courses.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddCourseModule();
        services.AddCategoryModule();
        return services;
    }

    private static void AddCategoryModule(this IServiceCollection services)
    {
        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
    }

    private static void AddCourseModule(this IServiceCollection services)
    {
        services.AddScoped<ICreateCourseUseCase, CreateCourseUseCase>();
        services.AddScoped<IUpdateCourseUseCase, UpdateCourseUseCase>();
    }
}