namespace AI_based_Language_Teaching.Service
{
    public class LanguageService : ILanguageService
    {
        public Dictionary<string, int> GetGrammarMap(string language)
        {
            
            switch (language.ToLower())
            {
                case "english":
                    return new Dictionary<string, int>
                    {
                        {"0",-1},
                        { "A1", 31 },
                        { "A2", 111 },
                        { "B1", 157 },
                        { "B2", 236 },
                        { "C1", 273 },
                        { "C2", 316 }
                    };
                case "french":
                    return new Dictionary<string, int>
                    {
                        {"0",-1},
                        { "A1", 47 },
                        { "A2", 97 },
                        { "B1", 148 },
                        { "B2", 198 },
                        { "C1", 248 },
                        { "C2", 289 }
                    };
                default:
                    throw new InvalidOperationException("No language found matching the requested value.");
            }
        }
        public Dictionary<string, int> GetwordMap(string language)
        {
            
            switch (language.ToLower())
            {
                case "english":
                    return new Dictionary<string, int>
                    {
                        {"0",-1},
                        { "A1", 1163 },
                        { "A2", 2574 },
                        { "B1", 5020 },
                        { "B2", 7798 },
                        { "C1", 8909 },
                        { "C2", 9934 }
                    };
                case "french":
                    return new Dictionary<string, int>
                    {
                        {"0",-1},
                        { "A1", 44 },
                        { "A2", 80 },
                        { "B1", 152 },
                        { "B2", 202 },
                        { "C1", 252 },
                        { "C2", 302 }
                    };
                default:
                    throw new InvalidOperationException("No language found matching the requested value.");
            }
        }
        public int GetTheLastGrammarLesson(string language)
        {
            switch (language.ToLower())
            {
                case "english":
                    return 316;
                case "french":
                    return 289;
                default:
                    throw new InvalidOperationException("No language found matching the requested value.");
            }
        }
        public int GetTheLastWordLesson(string language)
        {
           switch (language.ToLower())
            {
                case "english":
                    return 9934;
                case "french":
                    return 302;
                default:
                    throw new InvalidOperationException("No language found matching the requested value.");
            }
        }
        public string GetGrammarFileName(string language)
        {
            switch (language.ToLower())
            {
                case "english":
                    return "grammar_mappings.csv";
                case "french":
                    return "frenchGrammarMapping.csv";
                default:
                    throw new InvalidOperationException("No language found matching the requested value.");
            }
        }
         public string GetWordFileName(string language)
        {
            switch (language.ToLower())
            {
                case "english":
                    return "cefrj-vocabulary-profile-1.5.csv";
                case "french":
                    return "cefr-french-vocabulary-profile.csv";
                default:
                    throw new InvalidOperationException("No language found matching the requested value.");
            }
        }
    }
}