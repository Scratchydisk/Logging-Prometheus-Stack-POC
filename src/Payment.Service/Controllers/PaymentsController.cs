using Microsoft.AspNetCore.Mvc;
using Payment.Service.Models;

namespace Payment.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(ILogger<PaymentsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Models.Payment> Get()
    {
        _logger.LogInformation("Getting all payments");
        return new List<Models.Payment>
        {
            new Models.Payment { Id = 1, OrderId = 1, Amount = 1500.00m },
            new Models.Payment { Id = 2, OrderId = 2, Amount = 25.50m }
        };
    }
}