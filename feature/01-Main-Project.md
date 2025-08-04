Here’s a concrete plan for a **small, realistic POC setup** to explore **logging, log mining, alerting, and observability** in a C# microservices architecture with **Prometheus, Loki, and Grafana**.

---


## 🔬 **POC Services**

Keep it **small but realistic**:

1. **User Service** (CRUD: register user, get user info).
2. **Order Service** (create order, fetch orders for a user).
3. **Payment Service** (simulate payments for orders).
4. **BFF API**:`

   * Endpoint `/checkout` orchestrates:
     **Create order → Send "order\_created" → Payment Service consumes event → Marks paid.**
5. **Frontend UI**:

   * Login page.
   * Place order button.
   * View orders (status incl. paid/unpaid).

---

## 📝 **Logging (Loki)**

* Use **Serilog.Sinks.Loki** or **Grafana.Loki.Extensions**:

  ```csharp
  Log.Logger = new LoggerConfiguration()
      .Enrich.FromLogContext()
      .WriteTo.Console()
      .WriteTo.GrafanaLoki("http://localhost:3100") // Loki endpoint
      .CreateLogger();
  ```
* Include **correlation IDs** for tracing across services (`X-Correlation-ID` in headers).

---

## 📈 **Metrics (Prometheus)**

* Add **/metrics** endpoint to each service:

  ```csharp
  app.UseMetricServer(); // prometheus-net.AspNetCore
  Metrics.CreateCounter("orders_created", "Number of orders created");
  ```
* Grafana scrapes these via **Prometheus datasource**.

---

## 🔔 **Alerts**

* Create Grafana alerts:

  * **Error rate threshold** (e.g. >5% HTTP 500 in last 5m).
  * **High queue backlog** (custom Prometheus metric from Service Bus length).
  * **Service unavailability** (no scrape in X mins).

---

# Local Dev Setup

## 🐳 **docker-compose.yml**

```yaml
services:
  # --- Database ---
  mssql:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: mssql
    environment:
      SA_PASSWORD: "P@ssw0rd!"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
      - mssql_data:/var/opt/mssql

  # --- Message Bus ---
  rabbitmq:
    image: rabbitmq:3-management
    container_name: rabbitmq
    ports:
      - "5672:5672"      # AMQP
      - "15672:15672"    # Management UI
    environment:
      RABBITMQ_DEFAULT_USER: "guest"
      RABBITMQ_DEFAULT_PASS: "guest"
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq

  # --- Observability ---
  prometheus:
    image: prom/prometheus
    container_name: prometheus
    ports:
      - "9090:9090"
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml:ro
    command:
      - "--config.file=/etc/prometheus/prometheus.yml"

  loki:
    image: grafana/loki:2.9.0
    container_name: loki
    ports:
      - "3100:3100"
    command: -config.file=/etc/loki/config/config.yaml
    volumes:
      - ./loki-config.yaml:/etc/loki/config/config.yaml:ro

  grafana:
    image: grafana/grafana:10.2.2
    container_name: grafana
    ports:
      - "3000:3000"
    depends_on:
      - prometheus
      - loki
    volumes:
      - grafana_data:/var/lib/grafana
    environment:
      GF_SECURITY_ADMIN_USER: "admin"
      GF_SECURITY_ADMIN_PASSWORD: "admin"

volumes:
  mssql_data:
  rabbitmq_data:
  grafana_data:
```

---

## 🛠 **Prometheus Config (prometheus.yml)**

Scrapes **.NET metrics** (exposed on `/metrics`) and RabbitMQ metrics:

```yaml
global:
  scrape_interval: 5s

scrape_configs:
  - job_name: "dotnet-microservices"
    metrics_path: "/metrics"
    static_configs:
      - targets:
          - "bff:8080"
          - "userservice:8080"
          - "orderservice:8080"
          - "paymentservice:8080"

  - job_name: "rabbitmq"
    metrics_path: /metrics
    static_configs:
      - targets:
          - "rabbitmq:15692"  # Requires rabbitmq_prometheus plugin
```

➡ Enable RabbitMQ Prometheus plugin:

```bash
docker exec -it rabbitmq rabbitmq-plugins enable rabbitmq_prometheus
```

---

## 🗂 **Loki Config (loki-config.yaml)**

A minimal config:

```yaml
auth_enabled: false

server:
  http_listen_port: 3100

ingester:
  lifecycler:
    address: 127.0.0.1
    ring:
      kvstore:
        store: inmemory
    final_sleep: 0s
  chunk_idle_period: 5m
  chunk_retain_period: 30s

schema_config:
  configs:
    - from: 2023-01-01
      store: boltdb
      object_store: filesystem
      schema: v11
      index:
        prefix: index_
        period: 24h

storage_config:
  boltdb:
    directory: /loki/index

  filesystem:
    directory: /loki/chunks

limits_config:
  enforce_metric_name: false
```

---

## 🎯 **Resulting URLs**

* **Grafana** → [http://localhost:3000](http://localhost:3000) (user: admin / pass: admin)
* **Prometheus** → [http://localhost:9090](http://localhost:9090)
* **Loki API** → [http://localhost:3100/](http://localhost:3100/)
* **RabbitMQ Management** → [http://localhost:15672](http://localhost:15672) (guest/guest)
* **MSSQL** → `localhost,1433` (SQL auth: sa / P\@ssw0rd!)

---

## 🚀 **POC Development Steps**

1. **Bootstrap services**: scaffold each service as a .NET 8 Web API.
2. **Integrate MSSQL**: simple EF Core models per service.
3. **Add Azure Service Bus / RabbitMQ messaging**.
4. **Implement BFF orchestration**.
5. **Hook up Serilog → Loki** and metrics endpoints.
6. **Deploy Prometheus + Loki + Grafana via Docker Compose**.
7. **Configure Grafana dashboards + alerts**.

