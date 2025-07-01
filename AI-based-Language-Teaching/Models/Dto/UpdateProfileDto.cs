using System.ComponentModel.DataAnnotations;

namespace AI_based_Language_Teaching.Models.Dtos
{
    public class UpdateProfileDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}