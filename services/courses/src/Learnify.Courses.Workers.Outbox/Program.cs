using Learnify.Courses.Application;
using Learnify.Courses.Infrastructure.Messaging;
using Learnify.Courses.Infrastructure.Persistence;
using Learnify.Courses.Workers.Outbox;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<OutboxPublisherWorker>();

builder.Services.AddApplicationModule();
builder.Services.AddPersistenceModule(builder.Configuration);
builder.Services.AddMessagingModule(builder.Configuration);

var host = builder.Build();
await host.RunAsync();