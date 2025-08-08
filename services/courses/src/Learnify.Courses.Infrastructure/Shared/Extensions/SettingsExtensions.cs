using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learnify.Courses.Infrastructure.Shared.Extensions;

public static class SettingsExtensions
{
    public static T GetAndConfigureSettings<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName
    ) where T : class
    {
        services.AddOptions<T>()
            .BindConfiguration(sectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.GetSection(sectionName).Get<T>()
                       ?? throw new ArgumentException($"{typeof(T).Name} should be configured.");

        return settings;
    }
}