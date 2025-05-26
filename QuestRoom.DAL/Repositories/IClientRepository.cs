using QuestRoom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Client GetClientWithBookings(int id);
        Client GetClientWithCertificates(int id);
    }
}
