using Learnify.Courses.Application;
using Learnify.Courses.Infrastructure.Integrations;
using Learnify.Courses.Infrastructure.Persistence;
using Learnify.Courses.Infrastructure.Storaging;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationModule()
    .AddPersistenceModule(builder.Configuration)
    .AddStoragingModule(builder.Configuration)
    .AddIntegrationsModule(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Learnify - Courses API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

namespace Learnify.Courses.WebApi
{
    public abstract partial class Program
    {
    }
}