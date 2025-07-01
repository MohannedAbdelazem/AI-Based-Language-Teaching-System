namespace AI_based_Language_Teaching
{
    using AI_based_Language_Teaching.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}
        public DbSet<Curriculum> Curricula { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
