using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(int id);
        Task CreateQuestionAsync(Question question);
        Task UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(int id);
    }
}
