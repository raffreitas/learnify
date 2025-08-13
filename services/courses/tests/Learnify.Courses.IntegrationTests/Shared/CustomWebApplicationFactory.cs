using DotNet.Testcontainers.Builders;

using Learnify.Courses.Infrastructure.Persistence.Context;
using Learnify.Courses.Infrastructure.Persistence.Settings;
using Learnify.Courses.Infrastructure.Storage.Settings;
using Learnify.Courses.WebApi;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Testcontainers.Minio;
using Testcontainers.PostgreSql;

namespace Learnify.Courses.IntegrationTests.Shared;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("learnify")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(5432, true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .WithCleanUp(true)
        .Build();

    private readonly MinioContainer _minioContainer = new MinioBuilder()
        .WithImage("quay.io/minio/minio:latest")
        .WithPortBinding(9000, true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9000))
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        MapSettings(builder);
        builder.ConfigureTestServices(services =>
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        });

        base.ConfigureWebHost(builder);
    }

    private void MapSettings(IWebHostBuilder builder)
    {
        #region Postgres Settings

        builder.UseSetting(
            $"{DatabaseSettings.SectionName}:{nameof(DatabaseSettings.ConnectionString)}",
            _postgresContainer.GetConnectionString()
        );

        #endregion

        #region Minio Settings

        builder.UseSetting(
            $"{StorageSettings.SectionName}:{nameof(StorageSettings.Endpoint)}",
            _minioContainer.GetConnectionString()
        );
        builder.UseSetting(
            $"{StorageSettings.SectionName}:{nameof(StorageSettings.AccessKey)}",
            _minioContainer.GetAccessKey()
        );
        builder.UseSetting(
            $"{StorageSettings.SectionName}:{nameof(StorageSettings.SecretKey)}",
            _minioContainer.GetSecretKey()
        );

        #endregion
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        await _minioContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresContainer.StopAsync();
        await _postgresContainer.DisposeAsync();

        await _minioContainer.StopAsync();
        await _minioContainer.DisposeAsync();
    }
}