using QuestRoom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuestRoom.DAL.QuestRoom.DAL;

namespace QuestRoom.DAL.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(QuestRoomDbContext context) : base(context)
        {
        }

        public override Booking GetById(int id)
        {
            return _context.Bookings
                .Include(b => b.Quest)
                .Include(b => b.Client)
                .Include(b => b.UsedGiftCertificate)
                .FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Booking> GetBookingsByQuestId(int questId)
        {
            return _context.Bookings
                .Include(b => b.Client)
                .Where(b => b.QuestId == questId)
                .ToList();
        }

        public bool IsTimeSlotAvailable(int questId, DateTime startTime, DateTime endTime)
        {
            return !_context.Bookings
                .Any(b => b.QuestId == questId &&
                     b.Status != "Скасовано" &&
                     ((startTime >= b.StartTime && startTime < b.EndTime) ||
                      (endTime > b.StartTime && endTime <= b.EndTime) ||
                      (startTime <= b.StartTime && endTime >= b.EndTime)));
        }
    }

}
