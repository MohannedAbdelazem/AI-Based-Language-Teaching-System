using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
namespace AI_based_Language_Teaching.Models
{
    public class ApplicationUser : IdentityUser //need some modify in theeee future
    {
        public ICollection<Progress> Progresses { get; set; } = new List<Progress>();
        public ICollection<PracticeSession> PracticeSessions { get; set; } = new List<PracticeSession>();
    }
}
