using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface IAnswerService
    {
        IEnumerable<Answer> GetAnswers();
       Answer GetAnswerById(int id);
        void CreateAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void DeleteAnswer(int id);
    }
}