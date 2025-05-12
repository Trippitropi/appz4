using QuestRoom.DAL.Entities;
using QuestRoom.DAL.Repositories;
using System;

namespace QuestRoom.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Quest> Quests { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<Client> Clients { get; }
        IRepository<GiftCertificate> GiftCertificates { get; }

        // Спеціалізовані репозиторії
        QuestRepository QuestRepository { get; }
        BookingRepository BookingRepository { get; }
        ClientRepository ClientRepository { get; }
        GiftCertificateRepository GiftCertificateRepository { get; }

        int Complete();
    }
}