namespace AI_based_Language_Teaching.Models
{
    public class PracticeSession
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Type { get; set; } // Word, Spelling, Reading, etc, etc, etc..
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
