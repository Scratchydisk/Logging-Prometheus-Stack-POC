## Goal
To create a proof-of-concept (POC) solution demonstrating a C# microservices architecture with integrated logging, metrics, and observability using Prometheus, Loki, and Grafana.

## Why
This POC will serve as a blueprint for building robust, observable microservices. By establishing a solid foundation for logging and metrics, we can ensure that future services are easier to debug, monitor, and maintain.

## What
The project involves creating four microservices (User, Order, Payment, BFF API), a simple frontend UI, and a docker-compose setup for the observability stack (Prometheus, Loki, Grafana).

### Success Criteria
- [ ] All services are containerized and run via docker-compose.
- [ ] Logs from all services are shipped to Loki and are viewable in Grafana.
- [ ] Metrics from all services are scraped by Prometheus and are viewable in Grafana.
- [ ] A custom Grafana dashboard is created to visualize key metrics and logs.
- [ ] Alerts are configured in Grafana for critical events.

## All Needed Context

### Documentation & References
```yaml
# MUST READ - Include these in your context window
- url: https://github.com/serilog/serilog-sinks-grafana-loki
  why: Official documentation for the Serilog Loki sink.
- url: https://github.com/prometheus-net/prometheus-net
  why: Official documentation for the prometheus-net library.
- url: https://grafana.com/docs/loki/latest/
  why: Official Loki documentation.
- url: https://prometheus.io/docs/introduction/overview/
  why: Official Prometheus documentation.
- url: https://grafana.com/docs/grafana/latest/
  why: Official Grafana documentation.
- docfile: feature/01-Main-Project.md
  why: The original feature request document.
```

### Current Codebase tree
```bash
C:.
│   .gitignore
│   architecture.md
│   GEMINI.md
│
├───.gemini
│   ├───commands
│   │       execute-prp.md
│   │       generate-prp.md
│   │
│   └───templates
│           prp_template.md
│
├───feature
│       01-Main-Project.md
│
└───prps
```

### Desired Codebase tree with files to be added and responsibility of file
```bash
C:.
│   .dockerignore
│   .gitignore
│   architecture.md
│   docker-compose.yml
│   GEMINI.md
│   loki-config.yaml
│   prometheus.yml
│   README.md
│   Logging.sln
│
├───.gemini
│   ├───commands
│   │       execute-prp.md
│   │       generate-prp.md
│   │
│   └───templates
│           prp_template.md
│
├───feature
│       01-Main-Project.md
│
├───prps
│       01-Main-Project.md
│
└───src
    ├───Bff.Api
    │   │   Bff.Api.csproj
    │   │   Program.cs
    │   │
    │   └───Properties
    │           launchSettings.json
    │
    ├───Order.Service
    │   │   Order.Service.csproj
    │   │   Program.cs
    │   │
    │   └───Properties
    │           launchSettings.json
    │
    ├───Payment.Service
    │   │   Payment.Service.csproj
    │   │   Program.cs
    │   │
    │   └───Properties
    │           launchSettings.json
    │
    └───User.Service
        │   User.Service.csproj
        │   Program.cs
        │
        └───Properties
                launchSettings.json
```

### Known Gotchas of our codebase & Library Quirks
- The `Serilog.Sinks.Grafana.Loki` package is the recommended sink for Loki.
- `prometheus-net.AspNetCore` is the recommended library for exposing Prometheus metrics.
- Correlation IDs are essential for tracing requests across services.

## Implementation Blueprint

### Data models and structure
- Each service will have its own simple data models (e.g., User, Order, Payment).
- DTOs will be used for API requests and responses.

### list of tasks to be completed to fullfill the PRP in the order they should be completed

```yaml
Task 1:
CREATE solution file Logging.sln

Task 2:
CREATE src directory.

Task 3:
CREATE .NET 8 Web API projects for each service (User.Service, Order.Service, Payment.Service, Bff.Api) and add them to the solution.

Task 4:
CREATE docker-compose.yml, prometheus.yml, and loki-config.yaml files.

Task 5:
CREATE a simple frontend UI.

Task 6:
INTEGRATE Serilog.Sinks.Grafana.Loki into each service.

Task 7:
INTEGRATE prometheus-net.AspNetCore into each service.

Task 8:
IMPLEMENT the business logic for each service.

Task 9:
CONFIGURE Grafana with Prometheus and Loki data sources.

Task 10:
CREATE a Grafana dashboard to visualize metrics and logs.

Task 11:
CREATE Grafana alerts for critical events.
```

### Per task pseudocode as needed added to each task
```csharp
// Task 6
// In Program.cs for each service
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki("http://loki:3100")
    .CreateLogger();

builder.Host.UseSerilog();

// Task 7
// In Program.cs for each service
app.UseMetricServer();
app.UseHttpMetrics();

// In controllers
private static readonly Counter MyCounter = Metrics.CreateCounter("my_counter", "My custom counter");
MyCounter.Inc();
```

### Integration Points
```yaml
DATABASE:
  - MSSQL for data storage.

CONFIG:
  - appsettings.json for service-specific configuration.
  - docker-compose.yml for infrastructure configuration.

ROUTES:
  - Each service will expose its own API endpoints.
```

## Validation Loop

### Level 1: Syntax & Style
```bash
dotnet format --verify-no-changes
dotnet build --configuration Release
```

### Level 2: Unit Tests each new feature/file/function use existing test patterns
- Unit tests will be added for each service to verify business logic.

### Level 3: Integration Test
```bash
docker-compose up -d
# Test endpoints using curl or a tool like Postman
```

## Final Validation Checklist
- [ ] All services start and run without errors.
- [ ] Logs are visible in Grafana.
- [ ] Metrics are visible in Grafana.
- [ ] The Grafana dashboard displays relevant information.
- [ ] Alerts are triggered when conditions are met.
