using AI_based_Language_Teaching.Models;
namespace AI_based_Language_Teaching.Service
{
    public class ProgressService:IProgressService
    {
        private readonly ICurriculumService _curriculumService;
        private readonly IUserService _userServiece;
        private readonly ILessonService _lessonService;
        private readonly ILanguageService _languageService;
        private readonly IExerciseService _exersiseService;
        public ProgressService(ICurriculumService curriculumService,
        IUserService userServiece,
        ILessonService lessonService,
        ILanguageService languageService,
        IExerciseService exerciseService)
        {
            _curriculumService = curriculumService;
            _userServiece = userServiece;
            _lessonService = lessonService;
            _languageService = languageService;
            _exersiseService = exerciseService;
        }

        public async Task<Curriculum> FindCurriculum(long userID, string language)
        {
            User user = await _userServiece.GetUserByIdAsync(userID);
            List<Curriculum> curriculums = user.Curriculums;

            for (int i = 0; i < curriculums.Count; i++)
            {
                if (curriculums[i].Language == language)
                {
                    return curriculums[i];
                }
            }
            throw new InvalidOperationException($"No curriculum found for language '{language}' for user ID {userID}.");
        }
        public void CleanCurriculum(Curriculum curriculum, string type)
        {
            if (curriculum == null)
            {
                throw new ArgumentNullException(nameof(curriculum), "Curriculum cannot be null");
            }
            List<int> index;
            List<int> focus;
            List<int> newIndex = new List<int>();
            List<int> newFocus = new List<int>();
            if (type == "grammar")
            {
                index = curriculum.GrammarIndex;
                focus = curriculum.GrammarFocus;
            }
            else if (type == "word")
            {
                index = curriculum.WordsIndex;
                focus = curriculum.wordFocus;
            }
            else
            {
                throw new ArgumentException("Invalid type specified. Use 'grammar' or 'word'.", nameof(type));
            }
            for (int i = 0; i < index.Count; i++)
            {
                if (focus[i] > 1)
                {
                    newIndex.Add(index[i]);
                    newFocus.Add(focus[i]);
                }
            }
            if (type == "grammar")
            {
                curriculum.GrammarIndex = newIndex;
                curriculum.GrammarFocus = newFocus;
            }
            else if (type == "word")
            {
                curriculum.WordsIndex = newIndex;
                curriculum.wordFocus = newFocus;
            }
            _curriculumService.UpdateCurriculum(curriculum);
            return;

        }
        public async Task<string> NextGrammarLesson(long userID, string language, string chatHistory)
        {
            Curriculum curriculum = await FindCurriculum(userID, language);
            User user = await _userServiece.GetUserByIdAsync(userID);
            if (curriculum.LastGrammarLesson == _languageService.GetTheLastGrammarLesson(language))
            {
                throw new InvalidOperationException("no more grammar");
            }
            int nextLesson = curriculum.LastGrammarLesson + 1;
            string result;
            Dictionary<string, object> userinfo = new Dictionary<string, object>
            {
                { "name", user.firstName + " " + user.LastName },
                { "level", curriculum.grammarCefr },
                { "topics", user.preferences }
            };
            result = await _lessonService.GrammarLesson(nextLesson, chatHistory, userinfo, curriculum.Language);
            return result;
        }

        public async Task<string[]> NextReadingLesson(long userId, string language, string message, string chatHistory, string topic)
        {
            Curriculum curriculum = await FindCurriculum(userId, language);
            User user = await _userServiece.GetUserByIdAsync(userId);
            string[] result;
            Dictionary<string, object> user_data = new Dictionary<string, object>
            {
                { "name", user.firstName + " " + user.LastName },
                { "level", curriculum.grammarCefr },
                { "topics", user.preferences }
            };
            result = await _lessonService.ReadingLesson(user_data, chatHistory, message, topic, curriculum.Language);
            return result;
        }
       

        public async void addLessonToCurriculum(long userID, string language, int lesson, string type)
        {
            Curriculum curriculum = await FindCurriculum(userID, language);
            User user = await _userServiece.GetUserByIdAsync(userID);
            Dictionary<string, int> grammarMap = _languageService.GetGrammarMap(language);
            Dictionary<string, int> wordmap = _languageService.GetwordMap(language);
            Dictionary<string, string> promotMap = new Dictionary<string, string>
            {
                { "A1", "A2" },
                { "A2", "B1" },
                { "B1", "B2" },
                { "B2", "C1" },
                { "C1", "C2" },
                { "C2", "C2" }
            };
            if (type == "grammar")
            {
                if (lesson >= grammarMap[curriculum.grammarCefr])
                {
                    curriculum.grammarCefr = promotMap[curriculum.grammarCefr];
                    CleanCurriculum(curriculum, type);
                }
                curriculum.GrammarIndex.Add(lesson);
                curriculum.GrammarFocus.Add(1);
                curriculum.LastGrammarLesson = lesson;
            }
            else if (type == "word")
            {
                if (lesson >= wordmap[curriculum.wordCefr])
                {
                    curriculum.wordCefr = promotMap[curriculum.wordCefr];
                    CleanCurriculum(curriculum, type);
                }
                for (int i = curriculum.LastWordLesson + 1; i <= lesson; i++)
                {
                    curriculum.WordsIndex.Add(i);
                    curriculum.wordFocus.Add(1);
                }
                curriculum.LastWordLesson = lesson;
            }
            _curriculumService.UpdateCurriculum(curriculum);
        }
        public async Task<List<Dictionary<string, object>>> NexWordSession(long userID, string language)
        {
            Curriculum curriculum = await FindCurriculum(userID, language);
            User user = await _userServiece.GetUserByIdAsync(userID);
            if (curriculum.LastWordLesson == _languageService.GetTheLastWordLesson(language))
            {
                throw new InvalidOperationException("no more words");
            }
            Dictionary<string, object> userinfo = new Dictionary<string, object>
            {
                { "name", user.firstName + " " + user.LastName },
                { "level", curriculum.grammarCefr },
                { "topics", user.preferences }
            };
            int size = (int)curriculum.sessionSize;
            if (curriculum.LastWordLesson + size > _languageService.GetTheLastWordLesson(language)) size = _languageService.GetTheLastWordLesson(language) - curriculum.LastWordLesson;
            int[] words = new int[size];
            for (int i = 0; i < size; i++)
            {
                words[i] = curriculum.LastWordLesson + i + 1;
            }
            List<Dictionary<string, object>> result = await _lessonService.WordLesson(words, userinfo, curriculum.Language);
            return result;
        }
        public async Task<double> ProgressTracking(long userID, string language, string type)
        {
            Curriculum curriculum = await FindCurriculum(userID, language);
            User user = await _userServiece.GetUserByIdAsync(userID);
            Dictionary<string, int> GrammarMap = _languageService.GetGrammarMap(language);
            Dictionary<string, int> WordMap = _languageService.GetwordMap(language);
            Dictionary<string, string> downgradeMap = new Dictionary<string, string>
            {
                {"A1","0"},
                {"A2","A1"},
                {"B1","A2"},
                {"B2","B1"},
                {"C1","B2"},
                {"C2","C1"}
            };
            double result = 0.0;
            if (type == "grammar")
            {
                result = (double)(curriculum.LastGrammarLesson - GrammarMap[downgradeMap[curriculum.grammarCefr]]) / (double)(GrammarMap[curriculum.grammarCefr] - GrammarMap[downgradeMap[curriculum.grammarCefr]]);
            }
            else if (type == "word")
            {
                result = (double)(curriculum.LastWordLesson - WordMap[downgradeMap[curriculum.wordCefr]]) / (double)(WordMap[curriculum.wordCefr] - WordMap[downgradeMap[curriculum.wordCefr]]);
            }
            return result;
        }
        public async Task<List<Question>> SmallwordQuiz(int lesson, long userID, string language)
        {
            Curriculum curriculum = await FindCurriculum(userID, language);
            User user = await _userServiece.GetUserByIdAsync(userID);
            int size = lesson - curriculum.LastWordLesson;
            int[] index = new int[size];
            for (int i = 0; i < size; i++)
            {
                index[i] = curriculum.LastWordLesson + i + 1;
            }
            Question[] questions = await _exersiseService.WordExercise(index, curriculum.wordCefr, curriculum.Language);
            return questions.ToList();
        }
        public async Task<List<Question>> SmallGrammarQuiz(int lesson, long userID, string language)
        {
            Curriculum curriculum = await FindCurriculum(userID, language);
            User user = await _userServiece.GetUserByIdAsync(userID);
            int size = 5;
            int[] index = new int[size];
            for (int i = 0; i < size; i++)
            {
                index[i] = lesson;
            }
            Question[] questions = await _exersiseService.GrammarExercise(index, curriculum.grammarCefr, curriculum.Language);
            return questions.ToList();
        }
        public void addOtherTopics(int curriculumID, List<string> topics)
        {
            Curriculum curriculum = _curriculumService.GetCurriculumById(curriculumID);
            foreach (string temp in topics)
            {
                curriculum.otherTopics.Add(temp);
            }
            _curriculumService.UpdateCurriculum(curriculum);
        }
    }
}