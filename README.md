# C# Microservices Observability POC

## Overview

This repository contains a **proof-of-concept (POC)** microservices architecture implemented in C#/.NET 8, designed to demonstrate modern logging, metrics, alerting, and observability techniques using **Prometheus**, **Loki**, and **Grafana**.

The POC is intentionally small, yet realistic, and aims to showcase:

- Centralized **structured logging** with **correlation IDs** using **Serilog** piped to **Loki**.
- Service-level **Prometheus metrics** through `/metrics` endpoints.
- Real-time visualizations and **alerting** in **Grafana**.

This project is **also an experiment in AI context engineering**, using AI to plan, scaffold, and iteratively improve the architecture and codebase. All documentation and configuration are AI-assisted, with a focus on clear context and reproducibility.

---

## Architecture
- **User Service:** Handles user registration and info retrieval.
- **Order Service:** Manages orders, supports creating/fetching by user.
- **Payment Service:** Simulates processing payment for orders.
- **BFF (Backend For Frontend):** Provides a `/checkout` API orchestrating order creation and payment track events.
- **Frontend UI:** Simple interface for login, placing orders, and viewing paid/unpaid status.

All services use **.NET 8 Web APIs**.

---

## Observability Stack
- **Logging:** Structured logs are sent to **Loki** (via Serilog), enriched with correlation IDs for cross-service traceability.
- **Metrics:** Each service exposes a `/metrics` endpoint for **Prometheus** to scrape.
- **Dashboards/Alerts:** **Grafana** provides dashboards and alert rules (for error rates, service unavailability, backlogs, etc).

All observability stack services are managed locally via **Docker Compose**.

---

## Running Locally

### Getting Started

 To verify the logs and metrics in Grafana:

   1. Access Grafana: Open your web browser and navigate to http://localhost:3000. The default credentials are
      admin/admin. You will be prompted to change the password on first login.

   2. Verify Data Sources:
       * Once logged in, go to "Configuration" (gear icon on the left sidebar) -> "Data Sources".
       * You should see "Prometheus" and "Loki" data sources listed and configured.

   3. Verify Logs (Loki):
       * Go to "Explore" (compass icon on the left sidebar).
       * Select the "Loki" data source.
       * In the Log browser, you can enter $ git diff --cached to see logs from all services. You should see logs appearing
         from bff.api, order.service, payment.service, and user.service.

   4. Verify Metrics (Prometheus):
       * In "Explore", select the "Prometheus" data source.
       * Enter http_requests_received_total in the query field and run the query. You should see metrics data.

   5. Verify Custom Dashboard:
       * Go to "Dashboards" (dashboard icon on the left sidebar) -> "Browse".
       * You should find a dashboard named "BFF + Services API...". Click on it.
       * This dashboard should display both logs and the http_requests_received_total metric.

  To generate some logs and metrics, you can access the API endpoints:

   * Bff.Api: http://localhost:8080/swagger (and try the /Bff/user/{id} endpoint)
   * Order.Service: http://localhost:8081/swagger
   * Payment.Service: http://localhost:8082/swagger
   * User.Service: http://localhost:8083/swagger

  You can also directly access the Prometheus metrics endpoint for each service (e.g.,
  http://localhost:8080/metrics).

### Other References

See `docker-compose.yml` for setup:


- Prometheus: `localhost:9090`
- Loki: `localhost:3100`
- Grafana: `localhost:3000` (admin/admin)

See also:
- [`prometheus.yml`](./prometheus.yml): Prometheus targets/services scraped
- [`loki-config.yaml`](./loki-config.yaml): Simple, local Loki setup

---

## Development Process

1. Scaffolded individual .NET 8 microservices for user, order, payment, and BFF API.


4. Implemented structured logging and metrics endpoints.
5. Deployed observability stack (Prometheus + Loki + Grafana) via Docker Compose.
6. Configured Grafana dashboards and basic alert rules.

---

## About AI Context Engineering
This project doubles as a testbed for leveraging AI in both project planning and hands-on implementation. Documentation, setup, and feature breakdowns are AI-curated to maximize clarity and context for developers and ops engineers.

### How did the AI Fare?

I used Gemmini cli for most of the AI work.

In general it was very productive.

However it struggled to wire up Loki and Serilog properly; it really got stuck on setting up labels and it really got the version numbers in the docker-compose well out of date.

I tried GPT 4.1 on the labels issue but in the end I had to review the serilog-loki github repo to work out how the labels worked.

---

## Useful URLs
- Grafana: [http://localhost:3000](http://localhost:3000) (admin/admin)
- Prometheus: [http://localhost:9090](http://localhost:9090)
- Loki API: [http://localhost:3100/](http://localhost:3100/)



---

Feel free to use, experiment, and extend this project for learning, demos, or as a foundation for a production-grade observability stack in .NET microservices.