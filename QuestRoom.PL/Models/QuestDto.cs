namespace QuestRoom.PL.Models
{
    public class QuestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxParticipants { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public string DifficultyLevel { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }

}
