using QuestRoom.DAL.Entities;
using QuestRoom.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.BLL.Services
{
    public class ClientService
    {
        private readonly ClientRepository _clientRepository;

        public ClientService(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Client GetClientById(int id)
        {
            return _clientRepository.GetById(id);
        }

        public Client GetClientWithBookings(int id)
        {
            return _clientRepository.GetClientWithBookings(id);
        }

        public Client GetClientWithCertificates(int id)
        {
            return _clientRepository.GetClientWithCertificates(id);
        }

        public List<Client> GetAllClients()
        {
            return new List<Client>(_clientRepository.GetAll());
        }

        public void AddClient(Client client)
        {
            _clientRepository.Add(client);
            _clientRepository.SaveChanges();
        }

        public void UpdateClient(Client client)
        {
            _clientRepository.Update(client);
            _clientRepository.SaveChanges();
        }
    }


}
