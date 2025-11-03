using MagFlow.Web.Extensions;
using MagFlow.Web.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddMagFlowLogging();
builder.Configuration.MapToAppSettings();
builder.Services.AddMagFlowServices(builder.Configuration);

var app = builder.Build();
app.UseMagFlowPipeline();
app.MapEndpoints();
await app.SeedDatabase();

app.Run();

Log.CloseAndFlush();
