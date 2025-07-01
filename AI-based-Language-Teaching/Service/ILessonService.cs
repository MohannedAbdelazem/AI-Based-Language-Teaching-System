using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface ILessonService
    {
        public Task<List<Dictionary<string, object>>> WordLesson(int[] wordsindex, Dictionary<string, object> user_data, string language);
        public Task<string> GrammarLesson(int grammarIndex, string chatHistory, Dictionary<string, object> user_data, string language);
        public Task<string[]> ReadingLesson(Dictionary<string, object> user_data, string chatHistory, string userInput, string reading_topic, string language);

    }
}