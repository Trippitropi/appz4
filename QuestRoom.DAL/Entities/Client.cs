using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        // Навігаційна властивість для представлення зв'язку один-до-багатьох з бронюваннями
        public virtual ICollection<Booking> Bookings { get; set; }

        // Навігаційна властивість для представлення зв'язку один-до-багатьох з подарунковими сертифікатами
        // (Клієнт може володіти кількома сертифікатами)
        public virtual ICollection<GiftCertificate> OwnedCertificates { get; set; }

        public Client()
        {
            Bookings = new HashSet<Booking>();
            OwnedCertificates = new HashSet<GiftCertificate>();
        }


    }
}

