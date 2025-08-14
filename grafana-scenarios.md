## Grafana Dashboard Test Scenarios

These scenarios are designed to help prove out the functionality and accuracy of the Grafana dashboard by generating specific types of logs and metrics.

### 1. High Volume of Successful Requests

*   **Purpose:** Verify that the `http_requests_received_total` metric accurately reflects high traffic and that logs are ingested efficiently under load.
*   **Scenario:** Run the `test_api_endpoints.sh` script at `high` frequency for an extended period (e.g., 5-10 minutes).
    ```bash
    ./test_api_endpoints.sh -f high -d 300
    ```
*   **Verification:**
    *   Observe the `http_requests_received_total` metric in Grafana increasing rapidly and consistently.
    *   Check Loki for a high volume of `Information` level logs from all services.

### 2. Error Rate Spikes

*   **Purpose:** Verify that error logs are captured, and that error rate metrics (if configured) spike as expected.
*   **Scenario:** Continuously hit the `User.Service/error` endpoint. You can modify `test_api_endpoints.sh` to increase the probability of calling this specific endpoint, or run a separate `curl` loop targeting it.
    ```bash
    # Example of a separate curl loop for errors
    while true; do curl -s http://localhost:8083/Users/error > /dev/null; sleep 0.1; done
    ```
*   **Verification:**
    *   Look for `Error` level logs in Loki from `User.Service`.
    *   If your dashboard includes an error rate panel, observe it spiking.

### 3. Latency Impact

*   **Purpose:** Verify that the introduced latency for `User.Service/users/2` is visible in request duration metrics (if available) or by observing the impact on dependent services.
*   **Scenario:** Focus requests on `Bff.Api/user/2` and `User.Service/users/2`.
*   **Verification:**
    *   If the dashboard includes request duration metrics, observe an increase for calls involving `User.Service/users/2`.
    *   Check logs for any timeouts or increased processing times in `Bff.Api` when calling `User.Service/users/2`.

### 4. Service Unavailability/Restart

*   **Purpose:** Verify that the dashboard reflects service downtime and recovery.
*   **Scenario:** While the `test_api_endpoints.sh` script is running, stop one of the services (e.g., `docker stop bff.api`). After a short period, restart it (`docker start bff.api`).
    ```bash
    docker stop bff.api
    # ... wait some time ...
    docker start bff.api
    ```
*   **Verification:**
    *   Observe gaps in logs from the stopped service in Loki.
    *   If there are "service up/down" or "instance health" metrics, they should change accordingly.

### 5. Specific Log Levels

*   **Purpose:** Ensure that different log levels (Information, Warning, Error, Debug) are correctly captured and filterable in Loki.
*   **Scenario:** Manually add `Warning` or `Debug` level log statements to a controller method in one of the services, then trigger those methods.
    *Example (in C# code):*
    ```csharp
    _logger.LogWarning("This is a warning message for testing.");
    _logger.LogDebug("This is a debug message for testing.");
    ```
*   **Verification:**
    *   Filter logs in Grafana Loki by log level (`level="warn"` or `level="debug"`) to confirm they appear as expected.

### 6. Correlation ID Tracing

*   **Purpose:** Verify that logs from different services related to a single request can be traced using correlation IDs, as mentioned in the `README.md`.
*   **Scenario:** Make a request to `Bff.Api` (e.g., `/Bff/user/{id}`) that triggers calls to other services.
*   **Verification:**
    *   In Loki, search for the correlation ID associated with the initial `Bff.Api` request (you might need to inspect the logs for the correlation ID first).
    *   Confirm that logs from all involved services appear with that same ID, demonstrating end-to-end traceability.
