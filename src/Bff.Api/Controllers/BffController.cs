using Microsoft.AspNetCore.Mvc;

namespace Bff.Api.Controllers;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
}

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
}

[ApiController]
[Route("[controller]")]
public class BffController : ControllerBase
{
    private readonly ILogger<BffController> _logger;
    private readonly HttpClient _httpClient;

    public BffController(ILogger<BffController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        _logger.LogInformation("Getting user {UserId}", id);
        var user = await _httpClient.GetFromJsonAsync<User>($"http://user.service:8080/users/{id}");
        return Ok(user);
    }

    [HttpGet("orders/user/{userId}")]
    public async Task<IActionResult> GetOrdersForUser(int userId)
    {
        _logger.LogInformation("Getting orders for user {UserId}", userId);
        var orders = await _httpClient.GetFromJsonAsync<List<Order>>($"http://order.service:8080/orders?userId={userId}");
        return Ok(orders);
    }

    [HttpGet("payment/{orderId}")]
    public async Task<IActionResult> GetPaymentForOrder(int orderId)
    {
        _logger.LogInformation("Getting payment for order {OrderId}", orderId);
        var payment = await _httpClient.GetFromJsonAsync<Payment>($"http://payment.service:8080/payments?orderId={orderId}");
        return Ok(payment);
    }
}