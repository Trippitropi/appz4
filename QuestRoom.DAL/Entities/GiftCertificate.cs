using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Entities
{
    public class GiftCertificate
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsUsed { get; set; } 
        public DateTime ExpiryDate { get; set; } 

        
        public virtual Booking Booking { get; set; }

    }
}
