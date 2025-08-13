using Serilog;
using Prometheus;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    var lokiUrl = context.Configuration["LokiUrl"];
    if (string.IsNullOrWhiteSpace(lokiUrl))
    {
        throw new InvalidOperationException("LokiUrl configuration value is missing or empty.");
    }

    var applicationName = context.Configuration["ApplicationName"] ?? "Bff.Api";

    var labels = new List<LokiLabel>
    {
        new () {Key ="app", Value=applicationName },
        new () {Key ="env", Value=context.HostingEnvironment.EnvironmentName }
    };

    configuration
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", applicationName)
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .WriteTo.Console()
        .WriteTo.GrafanaLoki(
            lokiUrl,
            credentials: null,
            labels: labels);
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

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}