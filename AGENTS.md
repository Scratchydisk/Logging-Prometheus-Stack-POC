# Repository Guidelines

## Project Structure & Modules
- `src/Bff.Api`, `src/User.Service`, `src/Order.Service`, `src/Payment.Service`: .NET 8 Web APIs with Serilog + Prometheus.
- `docker-compose.yml`: Builds/runs services plus Loki, Prometheus, Grafana.
- `grafana/provisioning/**`: Dashboards and data sources.
- `prometheus.yml`, `loki-config.yaml`: Scrape and logging configs.
- `test_api_endpoints.sh`: Simple traffic generator for demos.
- `Logging.sln`: Root solution; use for restore/build.

## Build, Test, and Run
- Restore/build: `dotnet restore Logging.sln` then `dotnet build Logging.sln -c Debug`.
- Run a service locally: `dotnet run --project src/Bff.Api` (or any service dir).
- Start full stack: `docker compose up -d` (APIs: 8080–8083, Grafana: 3000, Prometheus: 9090, Loki: 3100).
- Generate load: `bash ./test_api_endpoints.sh -d 60 -f medium`.
- Verify: Swagger at `http://localhost:8080/swagger`, Grafana at `http://localhost:3000` (admin/admin).

## Coding Style & Naming
- C#: 4-space indent, `Nullable` and `ImplicitUsings` enabled (see `*.csproj`).
- Names: PascalCase for types/namespaces/methods; camelCase for locals/params; ALL_CAPS only for constants as needed.
- Logging: Use `ILogger<T>` and Serilog enrichers. Ensure `ApplicationName` and `LokiUrl` in `appsettings.Development.json`; labels `app` and `env` are applied for Loki queries.
- Endpoints: Prefer minimal APIs/controllers consistent with existing services; expose `/metrics` via `prometheus-net`.

## Testing Guidelines
- Integration smoke: run `test_api_endpoints.sh` while watching Grafana Explore and dashboards.
- Unit tests: not yet added. Prefer xUnit with project naming `{Service}.Tests` alongside the service; run via `dotnet test`.
- Keep tests fast, isolated, and deterministic; add metric/log assertions where practical.

## Commit & Pull Requests
- Commits: Imperative, concise subject, explain what/why (e.g., `Bff.Api: add Loki span enrichment`). Keep related changes together.
- PRs: Include summary, scope (service names), screenshots of dashboards if UI/observability changes, and links to issues. Note any config changes (`appsettings*`, compose, provisioning).

## Security & Config Tips
- Do not commit secrets. Use `appsettings.Development.json` for local values; production secrets via environment variables.
- Keep `LokiUrl` consistent with sink expectations (local compose uses base `http://loki:3100`).
- Ports in compose: 8080–8083 (APIs), 3000 (Grafana), 9090 (Prometheus), 3100 (Loki).
