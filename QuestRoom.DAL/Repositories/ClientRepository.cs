using QuestRoom.DAL.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuestRoom.DAL.QuestRoom.DAL;


namespace QuestRoom.DAL.Repositories
{
    public class ClientRepository : Repository<Client>
    {
        public ClientRepository(QuestRoomDbContext context) : base(context)
        {
        }

        // Отримуємо клієнта з його бронюваннями (жадібне завантаження)
        public Client GetClientWithBookings(int id)
        {
            return _context.Clients
                .Include(c => c.Bookings)
                    .ThenInclude(b => b.Quest)
                .FirstOrDefault(c => c.Id == id);
        }

        // Отримуємо клієнта з його сертифікатами
        public Client GetClientWithCertificates(int id)
        {
            return _context.Clients
                .Include(c => c.OwnedCertificates)
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
