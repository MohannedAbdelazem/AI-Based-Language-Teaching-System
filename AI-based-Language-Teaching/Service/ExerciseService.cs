using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace AI_based_Language_Teaching.Service
{



    public class GrammarCsvRow
    {
        public string rule { get; set; }
        public string cefr { get; set; }
    }

    public class GrammarExerciseSet
    {
        [JsonPropertyName("mcqs")]
        public List<GrammarExercise> GrammarExercises { get; set; }
    }

    public class GrammarExercise
    {
        public string question { get; set; }
        public Dictionary<string, string> options { get; set; }
        public string answer { get; set; }
    }
    //-------------------------------------------------------
    public class MyCsvRow
    {
        public string word { get; set; }
        public string pos { get; set; }
        public string cefr { get; set; }
    }
    public class WordExercise
    {
        [JsonPropertyName("mcqs")]
        public List<MCQExercise> MCQExercises { get; set; }
    }

    public class MCQExercise
    {
        public string question { get; set; }
        public Dictionary<string, string> options { get; set; }
        public string answer { get; set; }
    }

    public class ReadingExerciseSet
    {
        [JsonPropertyName("readings")]
        public List<ReadingExercise> ReadingExercises { get; set; }
    }
    public class ReadingExercise
    {
        public string passage { get; set; }
        public List<MCQExercise> questions { get; set; }
    }
    public class ExerciseRequestItem
    {
        public string cefr_level { get; set; }
        public int word_count { get; set; }
        public string topic { get; set; }
    }
    public class ExerciseService : IExerciseService
    {
        private readonly ILanguageService _languageService;

        private readonly UrlProvider _urlProvider;
        private readonly HttpClient _httpClient = new HttpClient();
        public ExerciseService(UrlProvider urlProvider, ILanguageService languageService)
        {
            _urlProvider = urlProvider;
            _languageService = languageService;
        }
        public object BuildJsonFromRows(MyCsvRow[] selectedRows,string Language)
        {
            return new { items = selectedRows,language=Language };
        }
        public object BuildJsonFromRows(GrammarCsvRow[] selectedRows,string Language)
        {
            return new
            {
                items = selectedRows,language=Language
            };
        }
        public static long GenerateUniqueId(int counter)
        {
            var now = DateTime.UtcNow;

            string timestamp = now.ToString("yyyyMMddHHmmss");

            long baseId = long.Parse(timestamp);
            // return baseId + counter;
            return baseId * 1000 + counter;
            // return counter;
        }
        public async Task<Question[]> WordExercise(int[] wordsindex, string level, string language)
        {
            using (var reader = new StreamReader(_languageService.GetWordFileName(language)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<MyCsvRow>().ToList();  // Read to List so we can use index

                var selectedRows = new List<MyCsvRow>();

                foreach (var index in wordsindex)
                {
                    if (index >= 0 && index < records.Count)
                    {
                        var row = records[index];
                        row.cefr = level;
                        selectedRows.Add(row);
                    }
                }
                object jsonRequest = BuildJsonFromRows(selectedRows.ToArray(), language);

                string jsonString = System.Text.Json.JsonSerializer.Serialize(jsonRequest);
                Console.WriteLine($"Response from API: {jsonString}");

                var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                string targetApiUrl = _urlProvider.FastApiUrl + "/generate-word-mcqs";

                var response = await _httpClient.PostAsync(targetApiUrl, content);

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response from API: {responseString}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return [];
                }
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                WordExercise MCQExerciseSet = JsonSerializer.Deserialize<WordExercise>(responseString, options);
                if (MCQExerciseSet == null || MCQExerciseSet.MCQExercises == null)
                {
                    Console.WriteLine("No exercises generated or deserialization failed.");
                    return [];
                }
                List<Question> questions = new List<Question>();
                int counter = 0;
                foreach (var MCQExercise in MCQExerciseSet.MCQExercises)
                {
                    if (MCQExercise == null)
                    {
                        Console.WriteLine($"⚠️ MCQExercise at index {counter} is null.");
                        continue;
                    }
                    Question question = new Question
                    {
                        Id = GenerateUniqueId(counter++),
                        Text = MCQExercise.question,
                        CorrectAnswer = MCQExercise.answer,
                        source_type = "word",
                        source_index = wordsindex[counter - 1],
                        options = new List<Option>()

                    };
                    if (MCQExercise.options != null)
                    {
                        foreach (var option in MCQExercise.options)
                        {
                            question.options.Add(new Option { Key = option.Key, Value = option.Value });
                        }
                    }
                    questions.Add(question);
                }
                Console.WriteLine($"Generated {questions.Count} questions.");
                foreach (var question in questions)
                {
                    Console.WriteLine($"Question ID: {question.Id}, Text: {question.Text}, Correct Answer: {question.CorrectAnswer}");
                }

                return questions.ToArray();
            }
        }



        public async Task<Question[]> GrammarExercise(int[] grammarIndex, string level, string language)
        {
            using (var reader = new StreamReader(_languageService.GetGrammarFileName(language)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<GrammarCsvRow>().ToList();  // Read to List so we can use index
                var selectedRows = new List<GrammarCsvRow>();
                foreach (var index in grammarIndex)
                {
                    if (index >= 0 && index < records.Count)
                    {
                        var row = records[index];
                        row.cefr = level;
                        selectedRows.Add(row);                    }
                }
                object jsonRequest = BuildJsonFromRows(selectedRows.ToArray(), language);
                string jsonString = System.Text.Json.JsonSerializer.Serialize(jsonRequest);
                Console.WriteLine($"Response from dataset: {jsonString}");
                var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                string targetApiUrl = _urlProvider.FastApiUrl + "/generate-grammar-mcqs";
                var response = await _httpClient.PostAsync(targetApiUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response from API: {responseString}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return [];
                }
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                GrammarExerciseSet grammarExerciseSet = JsonSerializer.Deserialize<GrammarExerciseSet>(responseString, options);
                if (grammarExerciseSet == null || grammarExerciseSet.GrammarExercises == null)
                {
                    Console.WriteLine("No exercises generated or deserialization failed.");
                    return [];
                }
                List<Question> questions = new List<Question>();
                int counter = 0;
                foreach (var grammarExercise in grammarExerciseSet.GrammarExercises)
                {
                    if (grammarExercise == null)
                    {
                        Console.WriteLine($"⚠️ grammarExercise at index {counter} is null.");
                        continue;
                    }
                    Question question = new Question
                    {
                        Id = GenerateUniqueId(counter++),
                        Text = grammarExercise.question,
                        CorrectAnswer = grammarExercise.answer,
                        source_type = "grammar",
                        source_index = grammarIndex[counter - 1],
                        options = new List<Option>()
                    };
                    if (grammarExercise.options != null)
                    {
                        foreach (var option in grammarExercise.options)
                        {
                            question.options.Add(new Option { Key = option.Key, Value = option.Value });
                        }
                    }
                    questions.Add(question);
                }
                Console.WriteLine($"Generated {questions.Count} questions.");
                foreach (var question in questions)
                {
                    Console.WriteLine($"Question ID: {question.Id}, Text: {question.Text}, Correct Answer: {question.CorrectAnswer}");
                }
                return questions.ToArray();
            }
        }
        public async Task<List<KeyValuePair<string, List<Question>>>> ReadingExercise(string cefrLevel, int[] wordCounts, string[] topics, string language)
        {
           
             var items = new List<ExerciseRequestItem>();

            for (int i = 0; i < topics.Length; i++)
            {
                items.Add(new ExerciseRequestItem
                {
                    cefr_level = cefrLevel,
                    word_count = wordCounts[i],
                    topic = topics[i]
                });
            }
            var requestBody = new
            {
                items = items
                ,language=language
            };

            string jsonString = System.Text.Json.JsonSerializer.Serialize(requestBody);
            Console.WriteLine($"Request to API: {jsonString}");

            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            string targetApiUrl = _urlProvider.FastApiUrl + "/generate-reading-mcqs";

            var response = await _httpClient.PostAsync(targetApiUrl, content);

            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response from API: {responseString}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return [];
            }
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            ReadingExerciseSet readingExerciseSet = JsonSerializer.Deserialize<ReadingExerciseSet>(responseString, options);
            if (readingExerciseSet == null || readingExerciseSet.ReadingExercises == null)
            {
                Console.WriteLine("No exercises generated or deserialization failed.");
                return [];
            }

            List<KeyValuePair<string, List<Question>>> readingQuestions = new List<KeyValuePair<string, List<Question>>>();
            int counter = 0;
            foreach (var readingExercise in readingExerciseSet.ReadingExercises)
            {
                if (readingExercise == null)
                {
                    Console.WriteLine($"⚠️ ReadingExercise at index {counter} is null.");
                    continue;
                }
                List<Question> questions = new List<Question>();
                foreach (var mcq in readingExercise.questions)
                {
                    if (mcq == null)
                    {
                        Console.WriteLine($"⚠️ MCQ at index {counter} is null.");
                        continue;
                    }
                    Question question = new Question
                    {
                        Id = GenerateUniqueId(counter++),
                        Text = mcq.question,
                        CorrectAnswer = mcq.answer,
                        source_type = "reading",
                        source_index = 0,
                        options = new List<Option>()
                    };
                    if (mcq.options != null)
                    {
                        foreach (var option in mcq.options)
                        {
                            question.options.Add(new Option { Key = option.Key, Value = option.Value });
                        }
                    }
                    questions.Add(question);
                }
                readingQuestions.Add(new KeyValuePair<string, List<Question>>(readingExercise.passage, questions));
            }
            Console.WriteLine($"Generated {readingQuestions.Count} reading exercises.");
            return readingQuestions;
        }
    }

}
