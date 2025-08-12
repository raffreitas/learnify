using Learnify.Catalog.Infrastructure.Messaging;
using Learnify.Catalog.Sync.Courses;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMessagingModule(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();