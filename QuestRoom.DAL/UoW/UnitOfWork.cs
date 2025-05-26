using QuestRoom.DAL.Entities;
using QuestRoom.DAL.QuestRoom.DAL;
using QuestRoom.DAL.Repositories;
using System;

namespace QuestRoom.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuestRoomDbContext _context;
        private bool _disposed = false;

        // Поля для загальних репозиторіїв
        private IRepository<Quest> _quests;
        private IRepository<Booking> _bookings;
        private IRepository<Client> _clients;
        private IRepository<GiftCertificate> _giftCertificates;

        // Поля для спеціалізованих репозиторіїв
        private IQuestRepository _questRepository;
        private IBookingRepository _bookingRepository;
        private IClientRepository _clientRepository;
        private IGiftCertificateRepository _giftCertificateRepository;

        public UnitOfWork(QuestRoomDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Властивості для загальних репозиторіїв з лінивою ініціалізацією
        public IRepository<Quest> Quests =>
            _quests ??= new Repository<Quest>(_context);

        public IRepository<Booking> Bookings =>
            _bookings ??= new Repository<Booking>(_context);

        public IRepository<Client> Clients =>
            _clients ??= new Repository<Client>(_context);

        public IRepository<GiftCertificate> GiftCertificates =>
            _giftCertificates ??= new Repository<GiftCertificate>(_context);

        // Властивості для спеціалізованих репозиторіїв з лінивою ініціалізацією
        public IQuestRepository QuestRepository =>
            _questRepository ??= new QuestRepository(_context);

        public IBookingRepository BookingRepository =>
            _bookingRepository ??= new BookingRepository(_context);

        public IClientRepository ClientRepository =>
            _clientRepository ??= new ClientRepository(_context);

        public IGiftCertificateRepository GiftCertificateRepository =>
            _giftCertificateRepository ??= new GiftCertificateRepository(_context);

        // Метод для збереження всіх змін
        public int Complete()
        {
            return _context.SaveChanges();
        }

        // Реалізація інтерфейсу IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
