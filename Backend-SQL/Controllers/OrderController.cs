using Backend_SQL.Data;
using Microsoft.AspNetCore.Mvc;
using Backend_SQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Backend_SQL.DTOs;


namespace Backend_SQL.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrdersController> _logger; 

        public OrdersController(AppDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _context.Orders
                .Select(o => new OrderDTO
                {
                    OrderID = o.OrderID,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    ToyID = o.ToyID,
                    ClothID = o.ClothID,
                    Email = o.Email,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    ToyName = o.Toy != null ? o.Toy.Name : null,
                    ClothName = o.Cloth != null ? o.Cloth.Name : null
                })
                .ToListAsync();

            return Ok(orders);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {

            var order = await _context.Orders
                .Include(o => o.Toy)
                .Include(o => o.Cloth)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderID} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully fetched order {OrderID}.", id);
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order {OrderID} created successfully.", order.OrderID);

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderID }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderID)
            {
                _logger.LogWarning("Order ID mismatch: {OrderID} does not match request ID {RequestID}.", order.OrderID, id);
                return BadRequest();
            }

            _logger.LogInformation("Updating order {OrderID}...", id);

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order {OrderID} updated successfully.", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(o => o.OrderID == id))
                {
                    _logger.LogWarning("Order {OrderID} not found during update.", id);
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order {OrderID} not found for deletion.", id);
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}







