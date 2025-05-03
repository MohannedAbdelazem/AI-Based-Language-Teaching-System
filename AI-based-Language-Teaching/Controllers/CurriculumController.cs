using AI_based_Language_Teaching.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurriculumController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CurriculumController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var curricula = await _context.Curricula.ToListAsync();
            return Ok(curricula);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.Id == id);
            if (curriculum == null)
                return NotFound();
            return Ok(curriculum);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Curriculum curriculum)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Curricula.Add(curriculum);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = curriculum.Id }, curriculum);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Curriculum curriculum)
        {
            if (id != curriculum.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Curricula.Update(curriculum);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.Id == id);
            if (curriculum == null)
                return NotFound();

            _context.Curricula.Remove(curriculum);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
