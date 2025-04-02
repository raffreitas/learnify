using Learnify.Courses.Api.Middleware;

using Serilog;

namespace Learnify.Courses.Api.Configuration;

public static class LoggingConfiguration
{
    public static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        return hostBuilder;
    }

    public static WebApplication UseLogging(this WebApplication app)
    {
        app.UseMiddleware<RequestLogContextMiddleware>();
        app.UseSerilogRequestLogging();
        return app;
    }
}
