using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_based_Language_Teaching.Service
{
    public class ProgressService : IProgressService
    {
        private readonly ApplicationDbContext _context;

        public ProgressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Progress> GetProgressByUserIdAsync(string userId)
        {
            return await _context.Progresses.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task UpdateProgressAsync(Progress progress)
        {
            _context.Progresses.Update(progress);
            await _context.SaveChangesAsync();
        }
    }

    }
