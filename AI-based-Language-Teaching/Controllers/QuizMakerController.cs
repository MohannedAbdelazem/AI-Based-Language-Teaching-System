using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizMakerController : ControllerBase
    {
        private readonly IQuizMaker _quizMaker;

        public QuizMakerController(IQuizMaker quizMaker)
        {
            _quizMaker = quizMaker;
        }

        [HttpPost("generate-random-quiz")]
        public IActionResult GenerateRandomQuiz([FromBody] QuizRequest request)
        {
            var result = _quizMaker.generateRandomQuiz(request.NumberOfQuestions, request.Curriculum, request.Type);
            return Ok(result);
        }

        [HttpPost("generate-focused-quiz")]
        public IActionResult GenerateFocusedQuiz([FromBody] QuizRequest request)
        {
            var result = _quizMaker.generateFocusedQuiz(request.NumberOfQuestions, request.Curriculum, request.Type);
            return Ok(result);
        }
    }

    public class QuizRequest
    {
        public int NumberOfQuestions { get; set; }
        public Curriculum Curriculum { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
    }
}