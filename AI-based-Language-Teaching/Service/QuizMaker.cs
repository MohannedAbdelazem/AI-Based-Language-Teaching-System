using AI_based_Language_Teaching.Models;
namespace AI_based_Language_Teaching.Service
{
    class QuizMaker : IQuizMaker
    {
        private readonly ILanguageService _languageService;
        public QuizMaker(ILanguageService languageService)
        {
            _languageService = languageService;
        }
        public int[] generateRandomQuiz(int numberOfQuestions, Curriculum curriculum, string type)
        {
            Random random = new Random();
            List<int> quizQuestions = new List<int>();

            if (type == "word")
            {
                for (int i = 0; i < numberOfQuestions; i++)
                {
                    int randomIndex = random.Next(0, curriculum.WordsIndex.Count);
                    quizQuestions.Add(curriculum.WordsIndex[randomIndex]);
                }
            }
            else if (type == "grammar")
            {
                for (int i = 0; i < numberOfQuestions; i++)
                {
                    int randomIndex = random.Next(0, curriculum.GrammarIndex.Count);
                    quizQuestions.Add(curriculum.GrammarIndex[randomIndex]);
                }
            }
            else if (type == "other")
            {
                for (int i = 0; i < numberOfQuestions; i++)
                {
                    int randomIndex = random.Next(0, curriculum.otherTopics.Count);
                    quizQuestions.Add(randomIndex); // since it's List<int> now
                }
            }

            return quizQuestions.ToArray();
        }

        public int[] generateFocusedQuiz(int numberOfQuestions, Curriculum curriculum, string type)
        {
            List<int> quizQuestions = new List<int>();
            Random random = new Random();

            if (type == "word")
            {
                List<int> originalFocus = new List<int>(curriculum.wordFocus);
                List<int> focusCopy = new List<int>(curriculum.wordFocus);

                int questionsGenerated = 0;

                while (questionsGenerated < numberOfQuestions)
                {
                    int totalFocus = focusCopy.Where(f => f > 0).Sum();

                    if (totalFocus == 0)
                    {
                        focusCopy = new List<int>(originalFocus);
                        continue;
                    }

                    int rand = random.Next(0, totalFocus);
                    int cumulative = 0;

                    for (int i = 0; i < curriculum.WordsIndex.Count; i++)
                    {
                        if (focusCopy[i] <= 0) continue;

                        cumulative += focusCopy[i];

                        if (rand < cumulative)
                        {
                            quizQuestions.Add(curriculum.WordsIndex[i]);
                            focusCopy[i] = Math.Max(0, focusCopy[i] - 1);
                            questionsGenerated++;
                            break;
                        }
                    }
                }
            }
            else if (type == "grammar")
            {
                List<int> originalFocus = new List<int>(curriculum.GrammarFocus);
                List<int> focusCopy = new List<int>(curriculum.GrammarFocus);

                int questionsGenerated = 0;

                while (questionsGenerated < numberOfQuestions)
                {
                    int totalFocus = focusCopy.Where(f => f > 0).Sum();

                    if (totalFocus == 0)
                    {
                        focusCopy = new List<int>(originalFocus);
                        continue;
                    }

                    int rand = random.Next(0, totalFocus);
                    int cumulative = 0;

                    for (int i = 0; i < curriculum.GrammarIndex.Count; i++)
                    {
                        if (focusCopy[i] <= 0) continue;

                        cumulative += focusCopy[i];

                        if (rand < cumulative)
                        {
                            quizQuestions.Add(curriculum.GrammarIndex[i]);
                            focusCopy[i] = Math.Max(0, focusCopy[i] - 1);
                            questionsGenerated++;
                            break;
                        }
                    }
                }
            }
            else if (type == "other")
            {
                List<int> originalFocus = new List<int>(curriculum.otherTopicsFocus);
                List<int> focusCopy = new List<int>(curriculum.otherTopicsFocus);

                int questionsGenerated = 0;

                while (questionsGenerated < numberOfQuestions)
                {
                    int totalFocus = focusCopy.Where(f => f > 0).Sum();

                    if (totalFocus == 0)
                    {
                        focusCopy = new List<int>(originalFocus);
                        continue;
                    }

                    int rand = random.Next(0, totalFocus);
                    int cumulative = 0;

                    for (int i = 0; i < curriculum.otherTopics.Count; i++)
                    {
                        if (focusCopy[i] <= 0) continue;

                        cumulative += focusCopy[i];

                        if (rand < cumulative)
                        {
                            quizQuestions.Add(i); // using index as the ID for otherTopics
                            focusCopy[i] = Math.Max(0, focusCopy[i] - 1);
                            questionsGenerated++;
                            break;
                        }
                    }
                }
            }

            return quizQuestions.ToArray();
        }
        public int[] GenerateRandomQuizWithinInterval(int numberOfQuestions, string language,string type, string start, string end)
        {
            Random random = new Random();
            List<int> quizQuestions = new List<int>();
            if (type == "word")
            {
                Dictionary<string, int> levels = _languageService.GetwordMap(language);
                int startIndex = levels[start] + 1;
                int endIndex = levels[end];
                Random randomIndex = new Random();
                for (int i = 0; i < numberOfQuestions; i++)
                {
                    int randomWordIndex = randomIndex.Next(startIndex, endIndex + 1);
                    quizQuestions.Add(randomWordIndex);
                }
            }
            else if (type == "grammar")
            {
                Dictionary<string, int> levels = _languageService.GetGrammarMap(language);
                for (int i = 0; i < numberOfQuestions; i++)
                {
                    int randomIndex = random.Next(levels[start], levels[end] + 1);
                    quizQuestions.Add(randomIndex);
                }

            }
           
            return quizQuestions.ToArray(); 
           
        }
    }
}
