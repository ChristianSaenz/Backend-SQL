using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_SQL.Models;
using Microsoft.Extensions.Logging;
using Backend_SQL.Data;
using Backend_SQL.DTO;

namespace Backend_SQL.Controllers
{
    [Route("api/toys")]
    [ApiController]
    public class ToyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ToyController> _logger;

        public ToyController(AppDbContext context, ILogger<ToyController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToyDTO>>> GetToys()
        {
            var toys = await _context.Toys
                .Select(t => new ToyDTO
                {
                    ToyID = t.ToyID,
                    Name = t.Name,
                    Quantity = t.Quantity
                })
                .ToListAsync();

            return Ok(toys);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Toy>> GetToy(int id)
        {

            var toy = await _context.Toys.FindAsync(id);

            if (toy == null)
            {
                _logger.LogWarning("Toy with ID {ToyID} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully fetched toy {ToyID}.", id);
            return toy;
        }

        [HttpPost]
        public async Task<ActionResult<Toy>> CreateToy(Toy toy)
        {

            _context.Toys.Add(toy);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Toy {ToyID} created successfully.", toy.ToyID);

            return CreatedAtAction(nameof(GetToy), new { id = toy.ToyID }, toy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToy(int id, Toy toy)
        {
            if (id != toy.ToyID)
            {
                _logger.LogWarning("Toy ID mismatch: {ToyID} does not match request ID {RequestID}.", toy.ToyID, id);
                return BadRequest();
            }

            _context.Entry(toy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Toy {ToyID} updated successfully.", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Toys.Any(t => t.ToyID == id))
                {
                    _logger.LogWarning("Toy {ToyID} not found during update.", id);
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToy(int id)
        {

            var toy = await _context.Toys.FindAsync(id);
            if (toy == null)
            {
                _logger.LogWarning("Toy {ToyID} not found for deletion.", id);
                return NotFound();
            }

            _context.Toys.Remove(toy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
