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

       
        public int? ClientId { get; set; }

      
        public int? QuestId { get; set; }

        
        [ForeignKey("ClientId")]
        public virtual Client Owner { get; set; }

      
        [ForeignKey("QuestId")]
        public virtual Quest Quest { get; set; }

        
        public virtual Booking UsedInBooking { get; set; }
    }
}
