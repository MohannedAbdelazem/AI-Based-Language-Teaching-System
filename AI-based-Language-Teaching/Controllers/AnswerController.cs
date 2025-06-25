using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var answers = _answerService.GetAnswers();
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();
            return Ok(answer);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Answer answer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _answerService.CreateAnswer(answer);
            return CreatedAtAction(nameof(Get), new { id = answer.Id }, answer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Answer answer)
        {
            if (id != answer.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _answerService.UpdateAnswer(answer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound();

            _answerService.DeleteAnswer(id);
            return NoContent();
        }
    }
}
