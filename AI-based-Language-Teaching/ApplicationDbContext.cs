namespace AI_based_Language_Teaching
{
    using AI_based_Language_Teaching.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}
        public DbSet<Curriculum> Curricula { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<PracticeSession> PracticeSessions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

    }
}
