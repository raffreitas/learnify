using Learnify.VideoProcessing.Application;
using Learnify.VideoProcessing.gRPC.Services;
using Learnify.VideoProcessing.Infrastructure.Persistence;
using Learnify.VideoProcessing.Infrastructure.Storaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationModule();
builder.Services.AddStorageModule(builder.Configuration);
builder.Services.AddPersistenceModule(builder.Configuration);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<UploadManagerService>();
app.MapGrpcService<VideosService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

await app.RunAsync();