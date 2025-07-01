using AI_based_Language_Teaching.Models;
namespace AI_based_Language_Teaching.Service
{

    public interface IProgressService
    {
        public Task<Curriculum> FindCurriculum(long userID, string language);
        public void CleanCurriculum(Curriculum curriculum, string type);
        public Task<string> NextGrammarLesson(long userID, string language, string chatHistory);
        public void addLessonToCurriculum(long userID, string language, int lesson, string type);
        public Task<List<Dictionary<string, object>>> NexWordSession(long userID, string language);
        public Task<double> ProgressTracking(long userID, string language, string type);
        public Task<List<Question>> SmallwordQuiz(int lesson, long userID, string language);
        public Task<List<Question>> SmallGrammarQuiz(int lesson, long userID, string language);
        public void addOtherTopics(int curriculumID, List<string> topics);
        public Task<string[]> NextReadingLesson(long userId, string language, string message, string chatHistory, string topic);





        }
}