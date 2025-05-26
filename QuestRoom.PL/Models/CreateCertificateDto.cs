using System.ComponentModel.DataAnnotations;

namespace QuestRoom.PL.Models
{
    public class CreateGiftCertificateDto
    {
        public int? ClientId { get; set; }
        public int? QuestId { get; set; }

        [Range(1, 365)]
        public int ValidityDays { get; set; } = 180;
    }
}
