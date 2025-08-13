using Serilog;
using Serilog.Sinks.Http.BatchFormatters;
using Serilog.Formatting.Compact;

using Prometheus;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Enable Serilog SelfLog
Serilog.Debugging.SelfLog.Enable(msg => File.AppendAllText("serilog-selflog.txt", msg + Environment.NewLine));

builder.Host.UseSerilog((context, services, configuration) =>
{
    var lokiUrl = context.Configuration["LokiUrl"];
    if (string.IsNullOrWhiteSpace(lokiUrl))
    {
        throw new InvalidOperationException("LokiUrl configuration value is missing or empty.");
    }

    var applicationName = context.Configuration["ApplicationName"] ?? "Not Specified";

    var labels = new List<LokiLabel>
    {
    new () {Key ="app", Value=applicationName },
    new () {Key ="env", Value=context.HostingEnvironment.EnvironmentName }
    };

    configuration
        .MinimumLevel.Debug()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", context.Configuration["ApplicationName"])
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
        .WriteTo.GrafanaLoki(
            lokiUrl,
            credentials: null,
            labels: labels);
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

// Add logging statements
app.Use(async (context, next) =>
{
    Log.Information("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next.Invoke();
    Log.Information("Finished handling request.");
});

app.Run();

