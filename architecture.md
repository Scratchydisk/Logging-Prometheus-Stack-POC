
## 🎯 **Goals**

* Multiple C# microservices communicating asynchronously (via **Azure Service Bus** or **local message bus** like RabbitMQ).
* **MSSQL** backend database usage.
* **Node.js frontend (simple UI)** calling a **BFF API** that orchestrates microservices.
* Integrated **logging (Serilog → Loki)** and **metrics (Prometheus)** pipelines.
* **Grafana dashboards and alerts** for logs and metrics.

---

## 🏗️ **Architecture Overview**

```
[ Node Frontend ]  -->  [ BFF (C# API) ]
                               |
         ------------------------------------------------
         |                  |                  |
 [ Service A: Orders ]  [ Service B: Users ]  [ Service C: Payments ]
         |                  |                  |
        (SQL)             (SQL)              (SQL)
         |
    [ Azure Service Bus / RabbitMQ ] (async events)
         |
   [ Centralized Loki logging + Prometheus metrics ]
         |
                  [ Grafana Dashboards & Alerts ]
```

---

## 🔧 **Technology Choices**

* **Backend (C#/.NET 8)**

  * Web APIs via **Minimal APIs** or **FastEndpoints**.
  * **Azure Service Bus SDK** or **MassTransit** for message bus abstraction.
  * **Serilog** (with **Loki sink**) for structured JSON logging.
  * **Prometheus-net** for exposing metrics endpoints.
* **Frontend (Node.js + Express + EJS/React)** for a very light UI.
* **DB**: Local **SQL Server (Docker)**.
* **Observability stack**:

  * **Loki** (log aggregation, queried via Grafana).
  * **Prometheus** (metrics scraping).
  * **Grafana** (dashboards, alerts).

---