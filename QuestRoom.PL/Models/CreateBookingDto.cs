using System.ComponentModel.DataAnnotations;

namespace QuestRoom.PL.Models
{
    public class CreateBookingDto
    {
        [Required]
        public int QuestId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(1, 20)]
        public int ParticipantsCount { get; set; }

        public string GiftCertificateCode { get; set; }
    }
}
