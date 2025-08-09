using Learnify.Courses.Application.Categories.UseCases.CreateCategory;
using Learnify.Courses.Application.Courses.UseCases.CreateCourse;
using Learnify.Courses.Application.Courses.UseCases.CreateModule;
using Learnify.Courses.Application.Courses.UseCases.CreateLesson;
using Learnify.Courses.Application.Courses.UseCases.PublishCourse;
using Learnify.Courses.Application.Courses.UseCases.SubmitCourseForReview;
using Learnify.Courses.Application.Courses.UseCases.UpdateCourse;
using Learnify.Courses.Application.Courses.UseCases.UpdateLesson;
using Learnify.Courses.Application.Courses.UseCases.UpdateModule;
using Learnify.Courses.Application.Courses.UseCases.ReorderModules;
using Learnify.Courses.Application.Courses.UseCases.ReorderLessons;

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
        services.AddScoped<IPublishCourseUseCase, PublishCourseUseCase>();
        services.AddScoped<ISubmitCourseForReviewUseCase, SubmitCourseForReviewUseCase>();

        services.AddScoped<ICreateModuleUseCase, CreateModuleUseCase>();
    services.AddScoped<IUpdateModuleUseCase, UpdateModuleUseCase>();
    services.AddScoped<ICreateLessonUseCase, CreateLessonUseCase>();
    services.AddScoped<IUpdateLessonUseCase, UpdateLessonUseCase>();
    services.AddScoped<IReorderModulesUseCase, ReorderModulesUseCase>();
    services.AddScoped<IReorderLessonsUseCase, ReorderLessonsUseCase>();
    }
}