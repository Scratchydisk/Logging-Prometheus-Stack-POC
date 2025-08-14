using Microsoft.AspNetCore.Mvc;
using User.Service.Models;

namespace User.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;

    private static readonly List<Models.User> _users = new List<Models.User>
    {
        new Models.User { Id = 1, Name = "Alice", Email = "alice@example.com" },
        new Models.User { Id = 2, Name = "Bob", Email = "bob@example.com" }
    };

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Models.User> Get()
    {
        _logger.LogInformation("Getting all users");
        return _users;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Models.User>> Get(int id)
    {
        _logger.LogInformation("Getting user by id {id}", id);

        if (id == 2)
        {
            _logger.LogInformation("Simulating 200ms latency for user 2");
            await Task.Delay(200); // Simulate 200ms latency
        }

        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        _logger.LogError("Generating a divide by zero error for testing purposes");
        int zero = 0;
        int result = 1 / zero; // This will throw a DivideByZeroException
    
        return StatusCode(500, "An error occurred");
    }
}
