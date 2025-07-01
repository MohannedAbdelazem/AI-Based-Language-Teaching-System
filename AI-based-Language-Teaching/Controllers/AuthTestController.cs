using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AI_based_Language_Teaching.Service;
namespace AI_based_Language_Teaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly JwtHelper _jwtHelper;
        public AuthTestController(IConfiguration configuration, JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        [HttpGet("getToken")]
        public async Task<IActionResult> getToken(){
            var token = _jwtHelper.GenerateToken("Mohanned", _configuration);
            return Ok(new { token });
        }
        [HttpGet("testToken")]
        [Authorize]
        public IActionResult TestToken()
        {
            return Ok("Token is valid");
        }
    }
}