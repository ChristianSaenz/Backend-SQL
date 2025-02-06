using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_SQL.Models;
using Microsoft.Extensions.Logging;
using Backend_SQL.Data;
using Backend_SQL.DTO;

namespace Backend_SQL.Controllers
{
    [Route("api/clothes")]
    [ApiController]
    public class ClothController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClothController> _logger;

        public ClothController(AppDbContext context, ILogger<ClothController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClothDTO>>> GetCloth()
        {
            var cloths = await _context.Cloths
                .Select(c => new ClothDTO
                {
                    ClothID = c.ClothID,
                    Name = c.Name,
                    Quantity = c.Quantity
                })
                .ToListAsync();

            return Ok(cloths);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Cloth>> GetCloth(int id)
        {

            var cloth = await _context.Cloths.FindAsync(id);

            if (cloth == null)
            {
                _logger.LogWarning("Cloth item with ID {ClothID} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully fetched cloth item {ClothID}.", id);
            return cloth;
        }

    
        [HttpPost]
        public async Task<ActionResult<Cloth>> CreateCloth(Cloth cloth)
        {
     
            _context.Cloths.Add(cloth);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Cloth item {ClothID} created successfully.", cloth.ClothID);

            return CreatedAtAction(nameof(GetCloth), new { id = cloth.ClothID }, cloth);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCloth(int id, Cloth cloth)
        {
            if (id != cloth.ClothID)
            {
                _logger.LogWarning("Cloth ID mismatch: {ClothID} does not match request ID {RequestID}.", cloth.ClothID, id);
                return BadRequest();
            } 

            _context.Entry(cloth).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cloth item {ClothID} updated successfully.", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cloths.Any(c => c.ClothID == id))
                {
                    _logger.LogWarning("Cloth item {ClothID} not found during update.", id);
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCloth(int id)
        {

            var cloth = await _context.Cloths.FindAsync(id);
            if (cloth == null)
            {
                _logger.LogWarning("Cloth item {ClothID} not found for deletion.", id);
                return NotFound();
            }

            _context.Cloths.Remove(cloth);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
