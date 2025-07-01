namespace AI_based_Language_Teaching.Models
{
    public class Curriculum
    {
        public int Id { get; set; }
        public List<int> WordsIndex { get; set; } // Array of word lessons indices
        public int LastWordLesson;
        public float sessionSize;
        public List<int> GrammarIndex { get; set; } // Array of grammar lessons indices
        public int LastGrammarLesson;
        public List<int> wordFocus { get; set; } // Array of focus for each word
        public List<int> GrammarFocus { get; set; } // Array of focus for each grammar lesson
        public string Language { get; set; } // Eng, French, German, etc.
        public List<string> otherTopics { get; set; }
        public List<int> otherTopicsFocus { get; set; } // Array of focus for each other topic
        public string wordCefr { get; set; }
        public string grammarCefr { get; set; }
        public string otherTopicsCefr { get; set; }
    }
}