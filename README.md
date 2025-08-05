# C# Microservices Observability POC

## Overview

This repository contains a **proof-of-concept (POC)** microservices architecture implemented in C#/.NET 8, designed to demonstrate modern logging, metrics, alerting, and observability techniques using **Prometheus**, **Loki**, and **Grafana**.

The POC is intentionally small, yet realistic, and aims to showcase:
- Service-to-service communication via messaging (RabbitMQ/Service Bus).
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

All services use **.NET 8 Web APIs** and connect to a common MSSQL database (via EF Core). Messaging is handled through **RabbitMQ**.

---

## Observability Stack
- **Logging:** Structured logs are sent to **Loki** (via Serilog), enriched with correlation IDs for cross-service traceability.
- **Metrics:** Each service exposes a `/metrics` endpoint for **Prometheus** to scrape. RabbitMQ metrics are exposed via plugin.
- **Dashboards/Alerts:** **Grafana** provides dashboards and alert rules (for error rates, service unavailability, backlogs, etc).

All observability stack services are managed locally via **Docker Compose**.

---

## Running Locally

See `docker-compose.yml` for setup:
- MSSQL (database): `localhost:1433`
- RabbitMQ (message bus): `localhost:5672`, Management UI `:15672`
- Prometheus: `localhost:9090`
- Loki: `localhost:3100`
- Grafana: `localhost:3000` (admin/admin)

See also:
- [`prometheus.yml`](./prometheus.yml): Prometheus targets/services scraped
- [`loki-config.yaml`](./loki-config.yaml): Simple, local Loki setup

---

## Development Process

1. Scaffolded individual .NET 8 microservices for user, order, payment, and BFF API.
2. Integrated EF Core data models and MSSQL.
3. Added messaging (RabbitMQ/Service Bus) for event-driven workflow.
4. Implemented structured logging and metrics endpoints.
5. Deployed observability stack (Prometheus + Loki + Grafana) via Docker Compose.
6. Configured Grafana dashboards and basic alert rules.

---

## About AI Context Engineering
This project doubles as a testbed for leveraging AI in both project planning and hands-on implementation. Documentation, setup, and feature breakdowns are AI-curated to maximize clarity and context for developers and ops engineers.

---

## Useful URLs
- Grafana: [http://localhost:3000](http://localhost:3000) (admin/admin)
- Prometheus: [http://localhost:9090](http://localhost:9090)
- Loki API: [http://localhost:3100/](http://localhost:3100/)
- RabbitMQ Management: [http://localhost:15672](http://localhost:15672) (guest/guest)
- MSSQL: `localhost,1433` (user: `sa`, password: `P@ssw0rd!`)

---

Feel free to use, experiment, and extend this project for learning, demos, or as a foundation for a production-grade observability stack in .NET microservices.