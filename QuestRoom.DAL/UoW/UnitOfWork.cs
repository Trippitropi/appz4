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

        // Загальні репозиторії
        private IRepository<Quest> _quests;
        private IRepository<Booking> _bookings;
        private IRepository<Client> _clients;
        private IRepository<GiftCertificate> _giftCertificates;

        // Спеціалізовані репозиторії
        private QuestRepository _questRepository;
        private BookingRepository _bookingRepository;
        private ClientRepository _clientRepository;
        private GiftCertificateRepository _giftCertificateRepository;

        public UnitOfWork(QuestRoomDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            _context = context;
        }

        // Властивості для загальних репозиторіїв
        public IRepository<Quest> Quests
        {
            get
            {
                if (_quests == null)
                {
                    _quests = new Repository<Quest>(_context);
                }
                return _quests;
            }
        }

        public IRepository<Booking> Bookings
        {
            get
            {
                if (_bookings == null)
                {
                    _bookings = new Repository<Booking>(_context);
                }
                return _bookings;
            }
        }

        public IRepository<Client> Clients
        {
            get
            {
                if (_clients == null)
                {
                    _clients = new Repository<Client>(_context);
                }
                return _clients;
            }
        }

        public IRepository<GiftCertificate> GiftCertificates
        {
            get
            {
                if (_giftCertificates == null)
                {
                    _giftCertificates = new Repository<GiftCertificate>(_context);
                }
                return _giftCertificates;
            }
        }

        // Властивості для спеціалізованих репозиторіїв
        public QuestRepository QuestRepository
        {
            get
            {
                if (_questRepository == null)
                {
                    _questRepository = new QuestRepository(_context);
                }
                return _questRepository;
            }
        }

        public BookingRepository BookingRepository
        {
            get
            {
                if (_bookingRepository == null)
                {
                    _bookingRepository = new BookingRepository(_context);
                }
                return _bookingRepository;
            }
        }

        public ClientRepository ClientRepository
        {
            get
            {
                if (_clientRepository == null)
                {
                    _clientRepository = new ClientRepository(_context);
                }
                return _clientRepository;
            }
        }

        public GiftCertificateRepository GiftCertificateRepository
        {
            get
            {
                if (_giftCertificateRepository == null)
                {
                    _giftCertificateRepository = new GiftCertificateRepository(_context);
                }
                return _giftCertificateRepository;
            }
        }

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