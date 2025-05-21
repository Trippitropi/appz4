using QuestRoom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.BLL.Services
{
    public interface IClientService
    {
        Client GetClientById(int id);
        Client GetClientWithBookings(int id);
        Client GetClientWithCertificates(int id);
        List<Client> GetAllClients();
        void AddClient(Client client);
        void UpdateClient(Client client);
    }
}
