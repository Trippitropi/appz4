using QuestRoom.DAL.Entities;
using QuestRoom.DAL.Repositories;
using System;

namespace QuestRoom.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Загальні репозиторії через базовий інтерфейс
        IRepository<Quest> Quests { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<Client> Clients { get; }
        IRepository<GiftCertificate> GiftCertificates { get; }

        // Спеціалізовані репозиторії через їхні власні інтерфейси
        IQuestRepository QuestRepository { get; }
        IBookingRepository BookingRepository { get; }
        IClientRepository ClientRepository { get; }
        IGiftCertificateRepository GiftCertificateRepository { get; }

        /// <summary>
        /// Зберігає всі зміни до бази даних
        /// </summary>
        /// <returns>Кількість записів, що змінились</returns>
        int Complete();
    }
}