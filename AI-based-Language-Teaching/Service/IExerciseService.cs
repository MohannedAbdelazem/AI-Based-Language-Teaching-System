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
    public interface IExerciseService
    {

        public Task<Question[]> WordExercise(int[] wordsindex, string level, string language);
        public Task<Question[]> GrammarExercise(int[] grammarIndex, string level, string language);
        public Task<List<KeyValuePair<string, List<Question>>>> ReadingExercise(string cefrLevel, int[] wordCounts, string[] topics, string language);
    }
}