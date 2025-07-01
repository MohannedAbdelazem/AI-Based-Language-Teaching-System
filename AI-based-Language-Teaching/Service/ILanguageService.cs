namespace AI_based_Language_Teaching.Service
{
    public interface ILanguageService
    {
        public Dictionary<string, int> GetGrammarMap(string language);

        public Dictionary<string, int> GetwordMap(string language);

        public int GetTheLastGrammarLesson(string language);

        public int GetTheLastWordLesson(string language);

        public string GetGrammarFileName(string language);

        public string GetWordFileName(string language);
        
    }
}