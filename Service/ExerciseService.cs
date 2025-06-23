using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AI_based_Language_Teaching.Service
{

    public class MyCsvRow
    {
        public string word { get; set; }
        public string pos { get; set; }
        public string cefr { get; set; }
    }
   public class WordExerciseSet
{
    [JsonPropertyName("mcqs")]
    public List<WordExercise> wordExercises { get; set; }
}

public class WordExercise
{
    public string question { get; set; }
    public Dictionary<string, string> options { get; set; }
    public  string answer { get; set; }
}

    public class ExerciseService
    {
        private readonly UrlProvider _urlProvider;
        private readonly HttpClient _httpClient = new HttpClient();
        public ExerciseService(UrlProvider urlProvider)
        {
            _urlProvider = urlProvider;
        }
        public object BuildJsonFromRows(MyCsvRow[] selectedRows)
        {
            return new { items = selectedRows };
        }
        public static long GenerateUniqueId(int counter)
        {
            var now = DateTime.UtcNow;

            string timestamp = now.ToString("yyyyMMddHHmmss");

            long baseId = long.Parse(timestamp);
            return baseId + counter;
            // return baseId*1000 + counter;
            // return counter;
        }
        public async Task<Question[]> WordExercise(int[] wordsindex, string level)
        {
            using (var reader = new StreamReader("cefrj-vocabulary-profile-1.5.csv"))
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
                object jsonRequest = BuildJsonFromRows(selectedRows.ToArray());

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

                WordExerciseSet wordExerciseSet = JsonSerializer.Deserialize<WordExerciseSet>(responseString, options);
                if (wordExerciseSet == null || wordExerciseSet.wordExercises == null)
                {
                    Console.WriteLine("No exercises generated or deserialization failed.");
                    return [];
                }
                List<Question> questions = new List<Question>();
                int counter = 0;
                foreach (var wordExercise in wordExerciseSet.wordExercises)
                {
                    if (wordExercise == null)
                    {
                        Console.WriteLine($"⚠️ wordExercise at index {counter} is null.");
                        continue;
                    }
                    Question question = new Question
                    {
                        Id = GenerateUniqueId(counter++),
                        Text = wordExercise.question,
                        CorrectAnswer = wordExercise.answer,
                        source_type = "word",
                        source_index = wordsindex[counter - 1],
                        options = new List<Dictionary<string, string>>()

                    };
                    if (wordExercise.options != null)
                    {
                        foreach (var option in wordExercise.options)
                        {
                            question.options.Add(new Dictionary<string, string> { { option.Key, option.Value } });
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

            
    }
}


