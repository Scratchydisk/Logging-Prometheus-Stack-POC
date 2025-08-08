# AGENT.md - C# Microservices Observability POC

## Build/Test Commands
- **Build:** `dotnet build` (from root)
- **Test:** `dotnet test` (no tests currently exist in codebase)
- **Run locally:** `docker-compose up` (full stack with observability)
- **Individual service:** `dotnet run` (from specific src/{Service} directory)

## Architecture
- **Main services:** 4 .NET 8 Web APIs: Bff.Api (8080), Order.Service (8081), Payment.Service (8082), User.Service (8083)  
- **Database:** MSSQL via Entity Framework Core, shared across services
- **Messaging:** RabbitMQ for async event-driven communication
- **Observability:** Prometheus metrics (/metrics endpoints), Serilog → Loki logging, Grafana dashboards
- **Infrastructure:** Docker Compose orchestrates services + observability stack (Prometheus:9090, Loki:3100, Grafana:3000)

## Code Style & Conventions
- **Language:** C# .NET 8, nullable reference types enabled (`<Nullable>enable</Nullable>`)
- **Namespaces:** File-scoped namespaces (e.g., `namespace Payment.Service.Controllers;`)
- **Controllers:** `[ApiController]` + `[Route("[controller]")]`, PascalCase naming, DI via constructor
- **Models:** Simple POCOs with auto-properties, PascalCase properties (e.g., `public int Id { get; set; }`)
- **Logging:** Structured logging via Serilog with DI (`ILogger<T> _logger`), use `_logger.LogInformation()`
- **Error handling:** Standard ASP.NET Core patterns, return appropriate HTTP status codes
