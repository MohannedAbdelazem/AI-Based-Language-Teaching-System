using AI_based_Language_Teaching.Models;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using System.IO;

namespace AI_based_Language_Teaching.Service
{

    public class MyCsvRow
    {
        public string word { get; set; }
        public string pos { get; set; }
        public string cefr { get; set; }
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
            return baseId * 1000 + counter;
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


            }
            return new Question[0];
        }
    }
}


