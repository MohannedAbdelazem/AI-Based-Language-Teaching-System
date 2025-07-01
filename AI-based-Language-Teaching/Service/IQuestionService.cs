using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface IQuestionService
    {
        IEnumerable<Question> GetQuestions();
        Question GetQuestionById(long id); 
        void CreateQuestion(Question question);
        void UpdateQuestion(Question question); 
        void DeleteQuestion(long id); 
    }
}