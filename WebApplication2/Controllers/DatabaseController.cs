using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly MyDbContextFactory _dbContextFactory;
        private static string _connectionString;
        private readonly IConfiguration _configuration;

        public DatabaseController(MyDbContextFactory dbContextFactory, IConfiguration configuration)
        {
            _dbContextFactory = dbContextFactory;
            _configuration = configuration;
        }

        [HttpPost("SetConnectionString")]
        public IActionResult SetConnectionString([FromBody] ConnectionStringModel model)
        {
            if (string.IsNullOrEmpty(model.ConnectionString))
            {
                return BadRequest("Connection string is required.");
            }

            _connectionString = model.ConnectionString;
            return Ok("Connection string set successfully.");
        }

        [HttpGet("GetConnectionString")]
        public string GetConnectionString()
        {
            return _connectionString ?? _configuration.GetConnectionString("MyDbConnectionStrings");
        }
    }

    public class ConnectionStringModel
    {
        public string ConnectionString { get; set; }
    }
}
