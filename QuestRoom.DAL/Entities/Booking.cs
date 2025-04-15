using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Required]
        public int ParticipantsCount { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public bool IsPaid { get; set; } = false;

        // Статус може бути: Очікує, Підтверджено, Завершено, Скасовано
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Очікує";

        // Зовнішні ключі
        [Required]
        public int QuestId { get; set; }

        [Required]
        public int ClientId { get; set; }

        public int? GiftCertificateId { get; set; }

        // Навігаційні властивості
        [ForeignKey("QuestId")]
        public virtual Quest Quest { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("GiftCertificateId")]
        public virtual GiftCertificate UsedGiftCertificate { get; set; }
    }
}
