namespace AI_based_Language_Teaching.Models
{
    public class Progress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        //public ApplicationUser User { get; set; }
        public int CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }
        public string Status { get; set; } // Not Started, In Progress, Completed
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
