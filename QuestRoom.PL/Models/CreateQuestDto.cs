using System.ComponentModel.DataAnnotations;

namespace QuestRoom.PL.Models
{
    public class CreateQuestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, 20)]
        public int MaxParticipants { get; set; }

        [Required]
        [Range(15, 300)]
        public int DurationMinutes { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [StringLength(20)]
        public string DifficultyLevel { get; set; }

        public string ImageUrl { get; set; }
    }
}
