using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace AI_based_Language_Teaching.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }
        public async Task<User> CreateUserAsync(string firstName, string lastNAme, string email, string plainPassword, List<string> preferences, DateTime birthDate)
        {
            var user = new User
            {
                firstName = firstName,
                LastName = lastNAme,
                email = email,
                preferences = preferences,
                birthDate = birthDate
            };
            user.passwordHash = _passwordHasher.HashPassword(user, plainPassword);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeleteUserAsync(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UserExistsAsync(long id) =>
            await _context.Users.AnyAsync(u => u.Id == id);

        public async Task<bool> UserExistsByEmailAsync(string email) =>
            await _context.Users.AnyAsync(u => u.email == email);

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.passwordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}