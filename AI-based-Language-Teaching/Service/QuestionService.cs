using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_based_Language_Teaching.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext _context;
        public QuestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Question> GetQuestions()
        {
            return _context.Questions.ToList();
        }

        public Question GetQuestionById(long id)
        {
            return _context.Questions.Find(id);
        }

        public void CreateQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public void UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
            _context.SaveChanges(); 
        }

        public void DeleteQuestion(long id)
        {
            var question = _context.Questions.Find(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges(); 
            }
        }
    }
}
