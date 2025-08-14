#!/bin/bash

# Default values
DURATION_SECONDS=-1 # -1 for forever
FREQUENCY="medium"  # low, medium, high

# Function to display usage
usage() {
    echo "Usage: $0 [-d <duration_in_seconds>] [-f <frequency_level>]"
    echo "  -d <duration_in_seconds>: How long to run the tests in seconds. Default: run forever."
    echo "  -f <frequency_level>: Frequency of calls. Options: low, medium, high. Default: medium."
    echo "        low: 1 call every 2 seconds"                                      
    echo "        medium: approx 3 calls/sec"                                       
    echo "        high: approx 10 calls/sec"  
    exit 1
}

# Parse command-line arguments
while getopts "d:f:" opt; do
    case "${opt}" in
        d) DURATION_SECONDS=${OPTARG} ;;
        f) FREQUENCY=${OPTARG} ;;
        *) usage ;;
    esac
done
shift $((OPTIND-1))

# Set sleep interval based on frequency
SLEEP_INTERVAL=0.3 # Default: medium (approx 3 calls/sec)
case "$FREQUENCY" in
    "low")    SLEEP_INTERVAL=2 ;;   # 1 call every 2  seconds
    "medium") SLEEP_INTERVAL=0.3 ;; # approx 3 calls/sec
    "high")   SLEEP_INTERVAL=0.1 ;; # approx 10 calls/sec
    *)
        echo "Invalid frequency level: $FREQUENCY. Using medium."
        SLEEP_INTERVAL=0.3
        ;;
esac

echo "Starting API test script..."
echo "Duration: $([ $DURATION_SECONDS -eq -1 ] && echo "Forever" || echo "${DURATION_SECONDS} seconds")"
echo "Frequency: $FREQUENCY (sleep interval: ${SLEEP_INTERVAL}s)"

START_TIME=$(date +%s)

# Define all API calls in an array
API_CALLS=(
    "curl -s http://localhost:8080/Bff/user/1 > /dev/null &"
    "curl -s http://localhost:8080/Bff/user/2 > /dev/null &"
    "curl -s http://localhost:8080/Bff/orders/user/1 > /dev/null &"
    "curl -s http://localhost:8080/Bff/orders/user/2 > /dev/null &"
    "curl -s http://localhost:8080/Bff/payment/1 > /dev/null &"
    "curl -s http://localhost:8080/Bff/payment/2 > /dev/null &"
    "curl -s http://localhost:8081/Orders > /dev/null &"
    "curl -s http://localhost:8082/Payments > /dev/null &"
    "curl -s http://localhost:8083/Users > /dev/null &"
    "curl -s http://localhost:8083/Users/1 > /dev/null &"
    "curl -s http://localhost:8083/Users/2 > /dev/null &"
    "curl -s http://localhost:8083/Users/error > /dev/null &"
)

# Get the number of API calls
NUM_API_CALLS=${#API_CALLS[@]}

# Loop for making API calls
while true; do
    CURRENT_TIME=$(date +%s)
    ELAPSED_TIME=$((CURRENT_TIME - START_TIME))

    if [ "$DURATION_SECONDS" -ne -1 ] && [ "$ELAPSED_TIME" -ge "$DURATION_SECONDS" ]; then
        echo "Duration reached. Exiting."
        break
    fi

    # Choose a random API call
    RANDOM_INDEX=$((RANDOM % NUM_API_CALLS))
    SELECTED_CALL=${API_CALLS[$RANDOM_INDEX]}

    echo "--- Making API call (Elapsed: ${ELAPSED_TIME}s) ---"
    eval "$SELECTED_CALL"

    sleep "$SLEEP_INTERVAL"
done