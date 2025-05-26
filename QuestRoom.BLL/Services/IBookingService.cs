using QuestRoom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.BLL.Services
{
    public interface IBookingService
    {
        Booking GetBookingById(int id);
        List<Booking> GetAllBookings();
        List<Booking> GetBookingsByQuestId(int questId);
        bool CreateBooking(int questId, int clientId, DateTime startTime, int participantsCount, string certificateCode = null);
        bool CancelBooking(int bookingId);
    }
}
