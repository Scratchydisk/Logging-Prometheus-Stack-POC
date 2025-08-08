using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Http.BatchFormatters;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    var lokiUrl = context.Configuration["LokiUrl"];
    Console.WriteLine($"Loki URL from appsettings: {lokiUrl}");
    configuration
        .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter());

    if (!string.IsNullOrWhiteSpace(lokiUrl))
    {
        configuration.WriteTo.Http(lokiUrl, queueLimitBytes: null, batchFormatter: new ArrayBatchFormatter());
    }
    else
    {
        Console.WriteLine("Warning: LokiUrl is not configured. Skipping Serilog HTTP sink setup.");
    }
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.MapMetrics();

app.Run();