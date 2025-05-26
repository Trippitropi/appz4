using QuestRoom.DAL.Entities;
using QuestRoom.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestRoom.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Client GetClientById(int id)
        {
            return _unitOfWork.ClientRepository.GetById(id);
        }

        public Client GetClientWithBookings(int id)
        {
            return _unitOfWork.ClientRepository.GetClientWithBookings(id);
        }

        public Client GetClientWithCertificates(int id)
        {
            return _unitOfWork.ClientRepository.GetClientWithCertificates(id);
        }

        public List<Client> GetAllClients()
        {
            return new List<Client>(_unitOfWork.Clients.GetAll());
        }

        public void AddClient(Client client)
        {
            _unitOfWork.Clients.Add(client);
            _unitOfWork.Complete();
        }

        public void UpdateClient(Client client)
        {
            _unitOfWork.Clients.Update(client);
            _unitOfWork.Complete();
        }
    }
}