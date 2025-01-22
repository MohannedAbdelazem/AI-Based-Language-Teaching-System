using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface IAnswerService
    {
        Task<IEnumerable<Answer>> GetAnswersAsync();
        Task<Answer> GetAnswerByIdAsync(int id);
        Task CreateAnswerAsync(Answer answer);
        Task UpdateAnswerAsync(Answer answer);
        Task DeleteAnswerAsync(int id);
    }
}
