using AI_based_Language_Teaching.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AI_based_Language_Teaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Add FirstName and LastName as Claims
                if (!string.IsNullOrWhiteSpace(model.FirstName))
                    await _userManager.AddClaimAsync(user, new Claim("FirstName", model.FirstName));

                if (!string.IsNullOrWhiteSpace(model.LastName))
                    await _userManager.AddClaimAsync(user, new Claim("LastName", model.LastName));

                return Ok(new { message = "User registered successfully." });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                // Optional: Return user claims for profile display
                var claims = await _userManager.GetClaimsAsync(user);
                var firstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
                var lastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value;

                return Ok(new
                {
                    message = "Login successful.",
                    firstName,
                    lastName
                });
            }

            return Unauthorized(new { message = "Invalid email or password." });
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var existingClaims = await _userManager.GetClaimsAsync(user);

            // Remove old claims if they exist
            var firstNameClaim = existingClaims.FirstOrDefault(c => c.Type == "FirstName");
            if (firstNameClaim != null)
                await _userManager.RemoveClaimAsync(user, firstNameClaim);

            var lastNameClaim = existingClaims.FirstOrDefault(c => c.Type == "LastName");
            if (lastNameClaim != null)
                await _userManager.RemoveClaimAsync(user, lastNameClaim);

            // Add updated claims
            await _userManager.AddClaimAsync(user, new Claim("FirstName", model.FirstName));
            await _userManager.AddClaimAsync(user, new Claim("LastName", model.LastName));

            return Ok(new { message = "Profile updated successfully." });
        }
    }
}