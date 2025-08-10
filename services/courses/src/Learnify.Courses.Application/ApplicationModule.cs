using Learnify.Courses.Application.Abstractions.Events;
using Learnify.Courses.Application.Abstractions.Events.Abstractions;
using Learnify.Courses.Application.Categories.UseCases.CreateCategory;
using Learnify.Courses.Application.Courses.Events.EventHandlers;
using Learnify.Courses.Application.Courses.UseCases.CreateCourse;
using Learnify.Courses.Application.Courses.UseCases.CreateModule;
using Learnify.Courses.Application.Courses.UseCases.CreateLesson;
using Learnify.Courses.Application.Courses.UseCases.GetCourseById;
using Learnify.Courses.Application.Courses.UseCases.PublishCourse;
using Learnify.Courses.Application.Courses.UseCases.UpdateCourse;
using Learnify.Courses.Application.Courses.UseCases.UpdateLesson;
using Learnify.Courses.Application.Courses.UseCases.UpdateModule;
using Learnify.Courses.Application.Courses.UseCases.ReorderModules;
using Learnify.Courses.Application.Courses.UseCases.ReorderLessons;
using Learnify.Courses.Application.Courses.UseCases.RequestCourseReview;
using Learnify.Courses.Application.Courses.UseCases.UploadCourseImage;
using Learnify.Courses.Domain.Aggregates.Courses.Events;
using Learnify.Courses.Domain.SeedWork;

using Microsoft.Extensions.DependencyInjection;

namespace Learnify.Courses.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
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
        services.AddScoped<IRequestCourseReviewUseCase, RequestCourseReviewUseCase>();
        services.AddScoped<IUploadCourseImageUseCase, UploadCourseImageUseCase>();
        services.AddScoped<IGetCourseByIdUseCase, GetCourseByIdUseCase>();

        services.AddScoped<ICreateModuleUseCase, CreateModuleUseCase>();
        services.AddScoped<IUpdateModuleUseCase, UpdateModuleUseCase>();
        services.AddScoped<ICreateLessonUseCase, CreateLessonUseCase>();
        services.AddScoped<IUpdateLessonUseCase, UpdateLessonUseCase>();
        services.AddScoped<IReorderModulesUseCase, ReorderModulesUseCase>();
        services.AddScoped<IReorderLessonsUseCase, ReorderLessonsUseCase>();

        services.AddScoped<
            IDomainEventHandler<RequestCourseReviewDomainEvent>,
            RequestCourseReviewDomainEventHandler
        >();
        services.AddScoped<IDomainEventHandler<CoursePublishedDomainEvent>, CoursePublishedDomainEventHandler>();
        services.AddScoped<IDomainEventHandler<CourseApprovedDomainEvent>, CourseApprovedDomainEventHandler>();
    }
}