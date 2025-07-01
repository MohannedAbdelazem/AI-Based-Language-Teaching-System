namespace AI_based_Language_Teaching.Models
{
    public class User
    {
        
        public long Id { get; set; }
        public string firstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public List<Curriculum> Curriculums { get; set; }
        public List<string> preferences { get; set; }
        public DateTime birthDate { get; set; }
    }
}
