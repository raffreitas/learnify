using Learnify.Identity.WebApi.Features.Users;
using Learnify.Identity.WebApi.Shared.Infrastructure.Persistence;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence();
builder.Services.AddUsersFeature();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseUsersFeature();

app.UseHttpsRedirection();

await app.RunAsync();