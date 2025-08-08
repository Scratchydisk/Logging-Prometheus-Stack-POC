using Microsoft.AspNetCore.Mvc;
using Order.Service.Models;

namespace Order.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Models.Order> Get()
    {
        _logger.LogInformation("Getting all orders");
        return new List<Models.Order>
        {
            new Models.Order { Id = 1, UserId = 1, ProductName = "Laptop", Quantity = 1 },
            new Models.Order { Id = 2, UserId = 2, ProductName = "Mouse", Quantity = 2 }
        };
    }
}