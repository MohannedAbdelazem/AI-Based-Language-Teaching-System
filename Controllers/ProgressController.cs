using AI_based_Language_Teaching.Models;
using AI_based_Language_Teaching.Service;
using Microsoft.AspNetCore.Mvc;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpGet("{userId}")]
        public IActionResult Get(string userId)
        {
            var progress = _progressService.GetProgressByUserId(userId);
            if (progress == null)
                return NotFound();

            return Ok(progress);
        }

        [HttpPut("{userId}")]
        public IActionResult Update(string userId, [FromBody] Progress progress)
        {
            if (userId != progress.UserId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _progressService.UpdateProgress(progress);
            return NoContent();
        }
    }
}
