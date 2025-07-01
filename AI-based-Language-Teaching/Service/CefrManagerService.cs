using AI_based_Language_Teaching.Models;

namespace AI_based_Language_Teaching.Service
{
    public class CefrManagerService : ICefrManagerService
    {
        private readonly IQuizMaker _quizMaker;
        private readonly IQuestionService _questionService;
        private readonly ICurriculumService _curriculumService;
        private readonly IExerciseService _exerciseService;

        public CefrManagerService(
            IQuizMaker quizMaker,
            IQuestionService questionService,
            ICurriculumService curriculumService,
            IExerciseService exerciseService)
        {
            _quizMaker = quizMaker;
            _questionService = questionService;
            _curriculumService = curriculumService;
            _exerciseService = exerciseService;
        }
        public async Task<List<Question>> CreateWordExercise(int CurriculumId, int numberOfQuestions)
        {
            Dictionary<string, string> CefrMap = new Dictionary<string, string>
            {
                { "A1", "A1" },
                { "A2", "A1" },
                { "B1", "A2" },
                { "B2", "B1" },
                { "C1", "B2" },
                { "C2", "C1" }
            };
            Curriculum curriculum = _curriculumService.GetCurriculumById(CurriculumId);
            if (curriculum == null)
            {
                throw new ArgumentException("Curriculum not found");
            }
            List<Question> questions;
            if (curriculum.wordCefr == "A1")
            {
                int[] totalWordIndex = _quizMaker.generateFocusedQuiz(numberOfQuestions, curriculum, "word");
                questions = (await _exerciseService.WordExercise(totalWordIndex, curriculum.wordCefr, curriculum.Language)).ToList();
            }
            else
            {
                int randomPart = (int)(numberOfQuestions * 0.2);
                int otherPart = numberOfQuestions - randomPart;
                int[] wordIndex = _quizMaker.generateFocusedQuiz(otherPart, curriculum, "word");
                int[] randomWordIndex = _quizMaker.GenerateRandomQuizWithinInterval(randomPart, curriculum.Language, "word", "0", CefrMap[curriculum.wordCefr]);
                int[] totalWordIndex = randomWordIndex.Concat(wordIndex).ToArray();
                questions = (await _exerciseService.WordExercise(totalWordIndex, curriculum.wordCefr, curriculum.Language)).ToList();
            }
            foreach (var question in questions)
            {
                _questionService.CreateQuestion(question);
                question.CorrectAnswer = "";
            }
            return questions;

        }
        public async Task<List<Question>> CreateGrammarExercise(int CurriculumId, int numberOfQuestions)
        {
            Dictionary<string, string> CefrMap = new Dictionary<string, string>
            {
                { "A1", "A1" },
                { "A2", "A1" },
                { "B1", "A2" },
                { "B2", "B1" },
                { "C1", "B2" },
                { "C2", "C1" }
            };
            Curriculum curriculum = _curriculumService.GetCurriculumById(CurriculumId);
            if (curriculum == null)
            {
                throw new ArgumentException("Curriculum not found");
            }
            List<Question> questions;
            if (curriculum.grammarCefr == "A1")
            {
                int[] totalGrammarIndex = _quizMaker.generateFocusedQuiz(numberOfQuestions, curriculum, "grammar");
                questions = (await _exerciseService.GrammarExercise(totalGrammarIndex, curriculum.grammarCefr, curriculum.Language)).ToList();
            }
            else
            {
                int randomPart = (int)(numberOfQuestions * 0.2);
                int otherPart = numberOfQuestions - randomPart;
                int[] grammarIndex = _quizMaker.generateFocusedQuiz(otherPart, curriculum, "grammar");
                int[] randomGrammarIndex = _quizMaker.GenerateRandomQuizWithinInterval(randomPart, curriculum.Language, "grammar", "0", CefrMap[curriculum.grammarCefr]);
                int[] totalGrammarIndex = randomGrammarIndex.Concat(grammarIndex).ToArray();
                questions = (await _exerciseService.GrammarExercise(totalGrammarIndex, curriculum.grammarCefr, curriculum.Language)).ToList();
            }
            foreach (var question in questions)
            {
                _questionService.CreateQuestion(question);
                question.CorrectAnswer = "";
            }
            return questions;

        }
        public List<KeyValuePair<long, bool>> CorrectionOfExercise(List<KeyValuePair<long, string>> questionsAndAnswers)
        {
            List<Question> questions = new List<Question>();
            List<string> answers = new List<string>();
            foreach (var qa in questionsAndAnswers)
            {
                Question question = _questionService.GetQuestionById(qa.Key);
                questions.Add(question);
                answers.Add(qa.Value);
            }
            List<KeyValuePair<long, bool>> results = new List<KeyValuePair<long, bool>>();
            for (int i = 0; i < questions.Count; i++)
            {
                bool isCorrect = questions[i].CorrectAnswer.Equals(answers[i], StringComparison.OrdinalIgnoreCase);
                results.Add(new KeyValuePair<long, bool>(questions[i].Id, isCorrect));
            }
            return results;
        }
        public void UpdateCurriculum(int curriculumId, List<KeyValuePair<long, bool>> questions)
        {
            Curriculum curriculum = _curriculumService.GetCurriculumById(curriculumId);
            if (curriculum == null)
            {
                throw new ArgumentException("Curriculum not found");
            }
            foreach (KeyValuePair<long, bool> question in questions)
            {
                long questionId = question.Key;
                bool value = question.Value;

                Question q = _questionService.GetQuestionById(questionId);
                if (q == null)
                {
                    throw new ArgumentException($"Question with ID {questionId} not found");
                }
                else
                {
                    string type = q.source_type;

                    if (type == "word")
                    {
                        int index = 0;
                        for (int i = 0; i < curriculum.WordsIndex.Count; i++)
                        {
                            if (curriculum.WordsIndex[i] == q.source_index)
                            {
                                index = i;
                                break;
                            }
                        }
                        if (value)
                        {
                            int focus = curriculum.wordFocus[index];
                            curriculum.wordFocus[index] = Math.Max(1, focus - 1);
                        }

                        else
                        {
                            int focus = curriculum.wordFocus[index];
                            curriculum.wordFocus[index] = Math.Min(1000, focus * 2);
                        }
                    }
                    else if (type == "grammar")
                    {
                        int index = 0;
                        for (int i = 0; i < curriculum.GrammarIndex.Count; i++)
                        {
                            if (curriculum.GrammarIndex[i] == q.source_index)
                            {
                                index = i;
                                break;
                            }
                        }
                        if (value)
                        {
                            int focus = curriculum.GrammarFocus[index];
                            curriculum.GrammarFocus[index] = Math.Max(1, focus - 1);
                        }
                        else
                        {
                            int focus = curriculum.GrammarFocus[index];
                            curriculum.GrammarFocus[index] = Math.Min(1000, focus * 2);
                        }
                    }
                }
            }
            _curriculumService.UpdateCurriculum(curriculum);

        }
        public async Task<List<KeyValuePair<string, List<Question>>>> CreateReadingExercise(int CurriculumId, int numberOfQuestions)
        {
            Curriculum curriculum = _curriculumService.GetCurriculumById(CurriculumId);
            string[] topics = new string[numberOfQuestions];
            int[] sizes = new int[numberOfQuestions];
            for (int j = 0; j < numberOfQuestions; j++)
            {
                sizes[j] = 250;
            }
            int i = 0;
            while (curriculum.otherTopics.Count > 0)
            {
                topics[i] = curriculum.otherTopics[0];
                curriculum.otherTopics.RemoveAt(0);
                i++;
            }
            _curriculumService.UpdateCurriculum(curriculum);
            while (i < numberOfQuestions)
            {
                topics[i] = "";
                i++;
            }
            List<KeyValuePair<string, List<Question>>> readingQuestions = await _exerciseService.ReadingExercise(curriculum.otherTopicsCefr, sizes, topics, curriculum.Language);
            foreach (var pair in readingQuestions)
            {
                string topic = pair.Key;
                List<Question> questions = pair.Value;

                foreach (var question in questions)
                {

                    _questionService.CreateQuestion(question);
                    question.CorrectAnswer = " ";
                }
            }
            return readingQuestions;

        }
       
    }
}