using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var questions = _questionService.GetQuestions();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
                return NotFound();

            return Ok(question);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Question question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _questionService.CreateQuestion(question);
            return CreatedAtAction(nameof(Get), new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Question question)
        {
            if (id != question.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _questionService.UpdateQuestion(question);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
                return NotFound();

            _questionService.DeleteQuestion(id);
            return NoContent();
        }
    }
}
