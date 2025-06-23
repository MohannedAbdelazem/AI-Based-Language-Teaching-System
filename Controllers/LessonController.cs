using AI_based_Language_Teaching.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LessonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("curriculum/{curriculumId}")]
        public IActionResult GetLessonsByCurriculum(int curriculumId)
        {
            var lessons = _context.Lessons.Where(l => l.CurriculumId == curriculumId).ToList();
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson == null)
                return NotFound();
            return Ok(lesson);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Lesson lesson)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Lessons.Add(lesson);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = lesson.Id }, lesson);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Lesson lesson)
        {
            if (id != lesson.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Lessons.Update(lesson);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.Id == id);
            if (lesson == null)
                return NotFound();

            _context.Lessons.Remove(lesson);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
