using QuestRoom.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuestRoom.DAL.QuestRoom.DAL;
namespace QuestRoom.DAL.Repositories
{
    public class QuestRepository : Repository<Quest>
    {
        public QuestRepository(QuestRoomDbContext context) : base(context)
        {
        }

        
        public Quest GetQuestWithBookings(int id)
        {
            return _context.Quests
                .Include(q => q.Bookings)
                .FirstOrDefault(q => q.Id == id);
        }
    }
}
