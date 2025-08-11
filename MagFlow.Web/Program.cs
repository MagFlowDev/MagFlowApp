using MagFlow.Web.Components;
using MagFlow.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMagFlowServices(builder.Configuration);

var app = builder.Build();

app.UseMagFlowPipeline();

app.Run();
