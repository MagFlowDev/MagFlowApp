using MagFlow.Web.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddMagFlowLogging();
builder.Services.AddMagFlowServices(builder.Configuration);

var app = builder.Build();
app.UseMagFlowPipeline();

app.Run();

Log.CloseAndFlush();

#if TEST

#endif