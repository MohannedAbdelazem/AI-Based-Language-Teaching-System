using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AI_based_Language_Teaching.Service
{
    public class JwtHelper
    {
        public string GenerateToken(string username, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = jwtSettings["Key"];
            Console.WriteLine($"Key: {key}"); // For debugging purposes, remove in production
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    // Add additional claims here if needed
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token valid for 2 hours
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = jwtSettings["Issuer"], // Since I have the validate issuer set to false then I don't need it for now
                Audience = jwtSettings["Audience"] // Since I have the validate audience set to false then I don't need it for now
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            return accessToken;
        }
    }
}