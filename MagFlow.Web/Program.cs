using MagFlow.BLL.Extensions;
using MagFlow.Web.Extensions;
using MagFlow.Web.Helpers;
using OpenTelemetry.Logs;
using Serilog;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var builder = WebApplication.CreateBuilder(args);
builder.AddMagFlowLogging();
builder.Configuration.MapToAppSettings();
builder.Services.AddMagFlowServices(builder.Configuration);
builder.Logging.AddOpenTelemetry(builder =>
{
    builder.IncludeFormattedMessage = true;
    builder.IncludeScopes = true;
    builder.ParseStateValues = true;
    builder.AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
});

var app = builder.Build();
app.UseMagFlowPipeline();
await app.InitializeDatabase();

app.MapEndpoints();
app.Run();

Log.CloseAndFlush();
