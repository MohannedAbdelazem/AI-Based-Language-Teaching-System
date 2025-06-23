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

        public  IEnumerable<Answer> GetAnswers()
        {
            return  _context.Answers.ToList();
        }

        public Answer GetAnswerById(int id)
        {
            return  _context.Answers.Find(id);
        }

        public void CreateAnswer(Answer answer)
        {
            _context.Answers.Add(answer);
             _context.SaveChanges();
        }

        public void UpdateAnswer(Answer answer)
        {
            _context.Answers.Update(answer);
            _context.SaveChanges();
        }

        public void DeleteAnswer(int id)
        {
            var answer =  _context.Answers.Find(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                 _context.SaveChangesAsync();
            }
        }
    }
}