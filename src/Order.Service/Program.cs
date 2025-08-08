using Serilog;
using Serilog.Sinks.Http.BatchFormatters;
using Serilog.Formatting.Compact;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    var lokiUrl = context.Configuration["LokiUrl"];
    if (string.IsNullOrWhiteSpace(lokiUrl))
    {
        throw new InvalidOperationException("LokiUrl configuration value is missing or empty.");
    }
    Console.WriteLine($"Loki URL from appsettings: {lokiUrl}");
    configuration
        .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
        .WriteTo.Http(lokiUrl, queueLimitBytes: null, batchFormatter: new ArrayBatchFormatter());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.MapMetrics();

app.Run();
