using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_based_Language_Teaching.Service
{
    public class AnswerService : IAnswerService
    {
        private readonly ApplicationDbContext _context;

        public AnswerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Answer>> GetAnswersAsync()
        {
            return await _context.Answers.ToListAsync();
        }

        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        public async Task CreateAnswerAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnswerAsync(Answer answer)
        {
            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
