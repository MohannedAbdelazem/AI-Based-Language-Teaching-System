using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_based_Language_Teaching.Service
{
    public class PracticeSessionService : IPracticeSessionService
    {
        private readonly ApplicationDbContext _context;

        public PracticeSessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PracticeSession>> GetSessionsAsync()
        {
            return await _context.PracticeSessions.ToListAsync();
        }

        public async Task<PracticeSession> GetSessionByIdAsync(int id)
        {
            return await _context.PracticeSessions.FindAsync(id);
        }

        public async Task StartSessionAsync(PracticeSession session)
        {
            _context.PracticeSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public async Task EndSessionAsync(int id)
        {
            var session = await _context.PracticeSessions.FindAsync(id);
            if (session != null)
            {
                session.EndTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
