using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace AI_based_Language_Teaching.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }
        [Authorize]
        [HttpPost("word-exercise")]
        public async Task<IActionResult> WordExercise([FromBody] WordExerciseRequest request)
        {
            var result = await _exerciseService.WordExercise(request.wordsindex, request.level, request.language);

            return Ok(result);
        }
        [HttpPost("grammar-exercise")]
        public async Task<IActionResult> GrammarExercise([FromBody] GrammarExerciseRequest request)
        {
            var result = await _exerciseService.GrammarExercise(request.wordsindex, request.level, request.language);

            return Ok(result);
        }
        [HttpPost("reading-exercise")]
        public async Task<IActionResult> ReadingExercise([FromBody] ReadingExerciseRequest request)
        {
            var result = await _exerciseService.ReadingExercise(request.level, request.word_counts, request.topics, request.language);

            return Ok(result);
        }
    }

    public class WordExerciseRequest
    {
        public int[] wordsindex { get; set; }
        public string level { get; set; }
        public string language { get; set; }
    }
    public class GrammarExerciseRequest
    {
        public int[] wordsindex { get; set; }
        public string level { get; set; }
        public string language { get; set; }
    }
    public class ReadingExerciseRequest
    {
        public string level { get; set; }
        public int[] word_counts { get; set; }
        public string[] topics { get; set; }
        public string language { get; set; }
    }
}