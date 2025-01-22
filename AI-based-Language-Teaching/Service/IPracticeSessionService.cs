using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface IPracticeSessionService
    {
        Task<IEnumerable<PracticeSession>> GetSessionsAsync();
        Task<PracticeSession> GetSessionByIdAsync(int id);
        Task StartSessionAsync(PracticeSession session);
        Task EndSessionAsync(int id);
    }
}
