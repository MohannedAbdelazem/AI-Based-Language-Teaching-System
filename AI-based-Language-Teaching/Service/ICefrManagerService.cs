using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public interface ICefrManagerService
    {
        public Task<List<Question>> CreateWordExercise(int CurriculumId, int numberOfQuestions) ;
        public Task<List<Question>> CreateGrammarExercise(int CurriculumId, int numberOfQuestions) ;
        public List<KeyValuePair<long, bool>> CorrectionOfExercise(List<KeyValuePair<long, string>> questionsAndAnswers) ;
        public void UpdateCurriculum(int curriculumId, List<KeyValuePair<long, bool>> questions) ;
        public Task<List<KeyValuePair<string, List<Question>>>> CreateReadingExercise(int CurriculumId, int numberOfQuestions) ;





    }
}