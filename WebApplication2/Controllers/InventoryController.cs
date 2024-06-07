using Microsoft.AspNetCore.Mvc;
using WebApplication1.DatabaseContext;
using WebApplication2.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly MyDbContextFactory _dbContextFactory;
        private readonly DatabaseController _databaseController;

        public InventoryController(MyDbContextFactory dbContextFactory, DatabaseController databaseController)
        {
            _dbContextFactory = dbContextFactory;
            _databaseController = databaseController;
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> AddProduct(Inventory inventory)
        {
            var connectionString = _databaseController.GetConnectionString();
            using (var _dbContext = _dbContextFactory.CreateDbContext(connectionString))
            {
                _dbContext.Inventory.Add(inventory);
                await _dbContext.SaveChangesAsync();
                return Ok(inventory);
            }
        }

        // Read All
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var connectionString = _databaseController.GetConnectionString();
            using (var _dbContext = _dbContextFactory.CreateDbContext(connectionString))
            {
                var inventory = await _dbContext.Inventory.ToListAsync();
                return Ok(inventory);
            }
        }

        // Read by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var connectionString = _databaseController.GetConnectionString();
            using (var _dbContext = _dbContextFactory.CreateDbContext(connectionString))
            {
                var inventory = await _dbContext.Inventory.FindAsync(id);
                if (inventory == null)
                {
                    return NotFound();
                }
                return Ok(inventory);
            }
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Inventory inventory)
        {
            if (id != inventory.InventoryID)
            {
                return BadRequest();
            }

            var connectionString = _databaseController.GetConnectionString();
            using (var _dbContext = _dbContextFactory.CreateDbContext(connectionString))
            {
                _dbContext.Entry(inventory).State = EntityState.Modified;

                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(_dbContext, id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var connectionString = _databaseController.GetConnectionString();
            using (var _dbContext = _dbContextFactory.CreateDbContext(connectionString))
            {
                var inventory = await _dbContext.Inventory.FindAsync(id);
                if (inventory == null)
                {
                    return NotFound();
                }

                _dbContext.Inventory.Remove(inventory);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
        }

        private bool InventoryExists(MyDbContext dbContext, int id)
        {
            return dbContext.Inventory.Any(e => e.InventoryID == id);
        }
    }
}
