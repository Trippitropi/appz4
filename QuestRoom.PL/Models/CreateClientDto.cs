using System.ComponentModel.DataAnnotations;

namespace QuestRoom.PL.Models
{
    public class CreateClientDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }
    }
}
