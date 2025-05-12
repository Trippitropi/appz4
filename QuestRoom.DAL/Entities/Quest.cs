using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuestRoom.DAL.Entities
{
    public class Quest
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

      
        [StringLength(20)]
        public string DifficultyLevel { get; set; }

        
        public virtual ICollection<Booking> Bookings { get; set; }

        public Quest()
        {
            Bookings = new HashSet<Booking>();
        }
    }
}
