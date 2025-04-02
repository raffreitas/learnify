using Learnify.Courses.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddOpenTelemetry();

builder.Host.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseLogging();

app.UseAuthorization();

app.MapControllers();

app.MapGet("ping", () => "pong");

await app.RunAsync();
