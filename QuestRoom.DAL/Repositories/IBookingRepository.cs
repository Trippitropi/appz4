using QuestRoom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        IEnumerable<Booking> GetBookingsByQuestId(int questId);
        bool IsTimeSlotAvailable(int questId, DateTime startTime, DateTime endTime);
    }
}
