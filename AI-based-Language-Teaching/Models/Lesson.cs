namespace AI_based_Language_Teaching.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; } // Lesson material
        public int CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }
    }
}
