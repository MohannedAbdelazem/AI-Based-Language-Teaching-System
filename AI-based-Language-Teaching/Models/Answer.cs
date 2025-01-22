namespace AI_based_Language_Teaching.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    }
}
