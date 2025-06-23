namespace AI_based_Language_Teaching.Models
{
    public class Question
    {
        
        public long Id { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public string source_type{ get; set; }
        public int source_index{ get; set; }
        public ICollection<Dictionary<string, string>> options { get; set; } = new List<Dictionary<string, string>>();
        
    }
}
