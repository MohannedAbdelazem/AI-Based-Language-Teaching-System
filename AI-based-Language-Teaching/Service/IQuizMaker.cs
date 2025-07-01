using AI_based_Language_Teaching.Models;
namespace AI_based_Language_Teaching.Service
{
    public interface IQuizMaker
    {
        public int[] generateRandomQuiz(int numberOfQuestions,Curriculum curriculum,  string type);


        public int[] generateFocusedQuiz(int numberOfQuestions, Curriculum curriculum, string type);
        public int[] GenerateRandomQuizWithinInterval(int numberOfQuestions,string language, string type, string start, string end);
        
    }
}
