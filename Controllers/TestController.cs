using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseService _exerciseService;

        public ExerciseController(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpPost("word-exercise")]
        public async Task<IActionResult> WordExercise([FromBody] WordExerciseRequest request)
        {
            var result = await _exerciseService.WordExercise(request.wordsindex, request.level);

            return Ok(result);
        }
    }

    public class WordExerciseRequest
    {
        public int[] wordsindex { get; set; }
        public string level { get; set; }
    }
}