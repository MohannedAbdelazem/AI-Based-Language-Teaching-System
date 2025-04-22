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

        public  Progress GetProgressByUserId(string userId)
        {
            return  _context.Progresses.FirstOrDefault(p => p.UserId == userId);
        }

        public void UpdateProgress(Progress progress)
        {
            _context.Progresses.Update(progress);
             _context.SaveChangesAsync();
        }
    }

}