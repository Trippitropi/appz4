using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; } 
        public DateTime StartTime { get; set; } 
        public DateTime EndTime { get; set; } 
        public int ParticipantsCount { get; set; } 
        public bool IsPaid { get; set; } 

        
        public int QuestId { get; set; }
        public int ClientId { get; set; }
        public int? GiftCertificateId { get; set; } 

        
        public virtual Quest Quest { get; set; }
        public virtual Client Client { get; set; }
        public virtual GiftCertificate GiftCertificate { get; set; }
    }
}
