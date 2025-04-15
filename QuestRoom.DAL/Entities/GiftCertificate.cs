using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Entities
{
    public class GiftCertificate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; } = false;

        // Зовнішній ключ для клієнта, який володіє сертифікатом
        public int? ClientId { get; set; }

        // Зовнішній ключ для квесту, для якого цей сертифікат дійсний (необов'язково, може бути для будь-якого квесту)
        public int? QuestId { get; set; }

        // Навігаційна властивість
        [ForeignKey("ClientId")]
        public virtual Client Owner { get; set; }

        // Навігаційна властивість
        [ForeignKey("QuestId")]
        public virtual Quest Quest { get; set; }

        // Навігаційна властивість до бронювання, де був використаний сертифікат (якщо таке є)
        public virtual Booking UsedInBooking { get; set; }
    }
}
