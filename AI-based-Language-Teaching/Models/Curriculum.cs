namespace AI_based_Language_Teaching.Models
{
    public class Curriculum
    {
        public int Id { get; set; }
        public string LanguageLevel { get; set; } // Beginner, Intermediate, Advanced
        public string Description { get; set; }
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
