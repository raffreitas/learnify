using System.Net.Http.Json;

using Bogus;

using Learnify.Courses.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Learnify.Courses.IntegrationTests.Shared;

public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    protected static Faker Faker => new();

    private readonly ApplicationDbContext _dbContext;
    private readonly HttpClient _httpClient;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (_dbContext.Database.GetPendingMigrations().Any())
        {
            _dbContext.Database.Migrate();
        }
    }

    protected async Task CleanUpDatabaseAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
    }

    protected async Task<HttpResponseMessage> PostAsync<T>(string method, T request)
    {
        return await _httpClient.PostAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> PutAsync<T>(string method, T request)
    {
        return await _httpClient.PutAsJsonAsync(method, request);
    }
}