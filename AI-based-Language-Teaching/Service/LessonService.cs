using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace AI_based_Language_Teaching.Service
{
    public class LessonService : ILessonService
    {
        private readonly UrlProvider _urlProvider;
        private readonly ILanguageService _languageService;
        private readonly HttpClient _httpClient = new HttpClient();
        public LessonService(UrlProvider urlProvider, ILanguageService languageService)
        {
            _urlProvider = urlProvider;
            _languageService = languageService;

        }
        public object BuildJsonFromRows(MyCsvRow[] selectedRows, string language)
        {
            return new { items = selectedRows, language = language };
        }
        public async Task<List<Dictionary<string, object>>> WordLesson(int[] wordsindex, Dictionary<string, object> user_data, string language)
        {
            using (var reader = new StreamReader(_languageService.GetWordFileName(language)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<MyCsvRow>().ToList();

                var selectedRows = new List<(int Index, MyCsvRow Row)>();

                foreach (var index in wordsindex)
                {
                    if (index >= 0 && index < records.Count)
                    {
                        selectedRows.Add((index, records[index]));
                    }
                }

                var resultList = new List<Dictionary<string, object>>();

                using (var client = new HttpClient())
                {
                    foreach (var (wordIndex, row) in selectedRows)
                    {
                        var word = row.word;
                        var pos = row.pos;

                        string defUrl = $"{_urlProvider.FastApiUrl}/definition/{language}/{Uri.EscapeDataString(word)}";
                        string definition = "";

                        try
                        {
                            var response = await client.GetAsync(defUrl);
                            response.EnsureSuccessStatusCode();

                            var json = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"[DEBUG] Response for '{word}': {json}");

                            var jsonDoc = JsonDocument.Parse(json);

                            if (jsonDoc.RootElement.TryGetProperty("definitions", out var defsProp) &&
                                defsProp.ValueKind == JsonValueKind.Array &&
                                defsProp.GetArrayLength() > 0)
                            {
                                definition = defsProp[0].GetString();
                            }
                            else
                            {
                                definition = "No definition found.";
                            }
                        }
                        catch (Exception ex)
                        {
                            definition = $"Error fetching definition: {ex.Message}";
                        }


                        string exampleUrl = $"{_urlProvider.FastApiUrl}/exampleWordPOS";
                        string example = "";

                        var examplePostBody = new
                        {
                            word = word,
                            pos = pos,
                            user_data = user_data,
                            language = language
                        };

                        try
                        {
                            var response = await client.PostAsJsonAsync(exampleUrl, examplePostBody);
                            response.EnsureSuccessStatusCode();

                            var json = await response.Content.ReadAsStringAsync();
                            var jsonDoc = JsonDocument.Parse(json);

                            if (jsonDoc.RootElement.TryGetProperty("example_sentence", out var exampleProp))
                            {
                                example = exampleProp.GetString();
                            }
                            else
                            {
                                example = "No example found.";
                            }
                        }
                        catch (Exception ex)
                        {
                            example = $"Error fetching example: {ex.Message}";
                        }

                        // Build dictionary for this word
                        var wordData = new Dictionary<string, object>
                        {
                            { "word_index", wordIndex },
                            { "word", word },
                            { "definition", definition },
                            { "example", example }
                        };

                        resultList.Add(wordData);
                    }
                }

                return resultList;
            }

        }
        public async Task<string> GrammarLesson(int grammarIndex, string chatHistory, Dictionary<string, object> user_data, string language)
        {
            using (var reader = new StreamReader(_languageService.GetGrammarFileName(language)))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<GrammarCsvRow>().ToList();

                if (grammarIndex < 0 || grammarIndex >= records.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(grammarIndex), "Invalid grammar index.");
                }

                var selectedRow = records[grammarIndex];
                string ruleText = selectedRow.rule;

                var postBody = new
                {
                    rule_text = ruleText,
                    user_data = user_data,
                    chat_string = chatHistory,
                    language = language
                };

                string apiUrl = $"{_urlProvider.FastApiUrl}/grammarChatBot ";

                try
                {
                    var response = await _httpClient.PostAsJsonAsync(apiUrl, postBody);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[DEBUG] GrammarExercise response: {json}");

                    var jsonDoc = JsonDocument.Parse(json);

                    if (jsonDoc.RootElement.TryGetProperty("chat_string", out var chatProp))
                    {
                        return chatProp.GetString();
                    }
                    else
                    {
                        return "No chat_string found in response.";
                    }
                }
                catch (Exception ex)
                {
                    return $"Error fetching grammar exercise: {ex.Message}";
                }
            }
        }
        public async Task<string[]> ReadingLesson(Dictionary<string, object> user_data, string chatHistory, string userInput, string reading_topic, string language)
        {
            string apiUrl = $"{_urlProvider.FastApiUrl}/chat";
            var postBody = new
            {
                message = userInput,
                user_data,
                reading_topic,
                language,
                chat_string = chatHistory
            };
            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, postBody);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] ReadingExercise response: {json}");

                var jsonDoc = JsonDocument.Parse(json);
                var root = jsonDoc.RootElement;

                var valuesList = new List<string>();
                foreach (var property in root.EnumerateObject())
                {
                    if (property.Name != "reply")
                    {
                        if (property.Value.ValueKind == JsonValueKind.Object || property.Value.ValueKind == JsonValueKind.Array)
                        {
                            valuesList.Add(property.Value.GetRawText());
                        }
                        else
                        {
                            valuesList.Add(property.Value.ToString());
                        }
                    }
                }
                return valuesList.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching reading exercise: {ex.Message}");
                return new string[] { "Error fetching reading exercise." };
            }


        }
    }
}