using Microsoft.AspNetCore.Mvc;
using AI_based_Language_Teaching.Service;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlTestController : ControllerBase
    {
        private readonly UrlProvider _urlProvider;

        public UrlTestController(UrlProvider urlProvider)
        {
            _urlProvider = urlProvider;
        }

        [HttpGet("fastapi-url")]
        public IActionResult GetFastApiUrl()
        {
            return Ok(new { fastApiUrl = _urlProvider.FastApiUrl });
        }
    }
}
