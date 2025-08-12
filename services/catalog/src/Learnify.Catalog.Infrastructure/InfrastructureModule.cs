using Learnify.Catalog.Core.Enums;
using Learnify.Catalog.Core.Repositories;
using Learnify.Catalog.Infrastructure.Persistence.Repositories;
using Learnify.Catalog.Infrastructure.Persistence.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

using OpenLibs.Extensions;

namespace Learnify.Catalog.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.ConfigureRequiredSettings<DatabaseSettings>(
            configuration,
            DatabaseSettings.SectionName
        );

        services.AddSingleton<IMongoClient>(_ =>
        {
            var clientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);

            clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());

            return new MongoClient(clientSettings);
        });

        services.AddSingleton<IMongoDatabase>(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new EnumSerializer<DifficultyLevel>(BsonType.String));

        services.AddScoped<ICourseRepository, CourseRepository>();
    }
}