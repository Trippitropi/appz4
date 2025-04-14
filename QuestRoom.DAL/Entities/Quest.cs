using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuestRoom.DAL.Entities
{
    public class Quest
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxParticipants { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Difficulty { get; set; }
        public string Theme { get; set; }


        public virtual ICollection<Booking> Bookings { get; set; }

    }
}
