namespace AI_based_Language_Teaching.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
