using Microsoft.AspNetCore.Mvc;
using AI_based_Language_Teaching.Service;
using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, JwtHelper jwtHelper, IConfiguration configuration)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromQuery] string firstName, [FromQuery] string LastName, [FromQuery] string email, [FromQuery] string password, [FromQuery] List<string> preferences, [FromQuery] DateTime birthDate)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return BadRequest("Password must be at least 6 characters long.");

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(LastName))
                return BadRequest("First name and last name are required.");
            
            if (await _userService.UserExistsByEmailAsync(email))
                return BadRequest("Email already registered.");
            
            User user = await _userService.CreateUserAsync(firstName, LastName, email, password, preferences, birthDate);

            if (user == null)
                return BadRequest("User registration failed.");

            var token = _jwtHelper.GenerateToken(email, _configuration);
            return Ok(new { token, user = user });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            var user = await _userService.LoginAsync(email, password);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var token = _jwtHelper.GenerateToken(email, _configuration);
            return Ok(new { token, user });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            return deleted ? Ok("User deleted.") : NotFound("User not found.");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (!await _userService.UserExistsAsync(user.Id))
                return NotFound("User not found.");

            var updatedUser = await _userService.UpdateUserAsync(user);
            return Ok(updatedUser);
        }
    }
}