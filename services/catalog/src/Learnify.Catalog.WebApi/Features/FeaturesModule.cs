using Learnify.Catalog.WebApi.Features.CreateCourse;

namespace Learnify.Catalog.WebApi.Features;

public static class FeaturesModule
{
    public static IServiceCollection AddCourseFeatures(this IServiceCollection services)
    {
        services.AddScoped<ICreateCourseUseCase, CreateCourseUseCase>();


        return services;
    }
}