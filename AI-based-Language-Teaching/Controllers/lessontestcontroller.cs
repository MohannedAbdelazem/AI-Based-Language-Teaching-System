using Microsoft.AspNetCore.Mvc;
using AI_based_Language_Teaching.Service;
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly LessonService _lessonService;

    public TestController(LessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpPost("wordlesson")]
    public async Task<IActionResult> GenerateLesson([FromBody] WordLessonRequest request)
    {
        if (request.WordsIndex == null || request.UserData == null)
        {
            return BadRequest("Invalid request.");
        }

        var results = await _lessonService.WordLesson(request.WordsIndex, request.UserData, request.language);

        return Ok(results);
    }
    [HttpPost("grammar")]
    public async Task<IActionResult> GenerateGrammarExercise([FromBody] GrammarLessonRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid request.");
        }

        var chatString = await _lessonService.GrammarLesson(
            request.GrammarIndex,
            request.ChatString,
            request.UserData,
            request.language
        );

        return Ok(new { chat_string = chatString });
    }
}
public class WordLessonRequest
{
    public int[] WordsIndex { get; set; }
    public Dictionary<string, object> UserData { get; set; }
    public string language { get; set; } = "English";
}
public class GrammarLessonRequest
{
    public int GrammarIndex { get; set; }
    public string ChatString { get; set; }
    public Dictionary<string, object> UserData { get; set; }
    public string language { get; set; } = "English";
}