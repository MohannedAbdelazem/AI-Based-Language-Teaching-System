using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByEmailAsync(string email);
        public Task<User> CreateUserAsync(string firstName, string lastNAme, string email, string plainPassword, List<string> preferences, DateTime birthDate);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(long id);
        Task<bool> UserExistsAsync(long id);
        Task<bool> UserExistsByEmailAsync(string email);
        public Task<User?> LoginAsync(string email, string password);
        
    }
}