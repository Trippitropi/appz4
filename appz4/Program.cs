using System;
using System.Linq;
using appz4;
using Microsoft.EntityFrameworkCore;
using QuestRoom.DAL;
using QuestRoom.DAL.Entities;
using QuestRoom.DAL.QuestRoom.DAL;
using QuestRoom.DAL.Repositories;

class Program
{
   
    private static QuestService _questService;
    private static ClientService _clientService;
    private static BookingService _bookingService;
    private static GiftCertificateService _certificateService;
    private static QuestRoomDbContext _dbContext;

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        InitializeServices();
        SeedData();

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== Система управління квест-кімнатами ===");
            Console.WriteLine("1. Переглянути всі квести");
            Console.WriteLine("2. Переглянути всі бронювання");
            Console.WriteLine("3. Створити нове бронювання");
            Console.WriteLine("4. Створити подарунковий сертифікат");
            Console.WriteLine("5. Скасувати бронювання");
            Console.WriteLine("6. Додати нового клієнта");
            Console.WriteLine("0. Вихід");
            Console.Write("\nВиберіть опцію: ");

            var option = Console.ReadLine();
            Console.WriteLine();

            switch (option)
            {
                case "1":
                    ShowAllQuests();
                    break;
                case "2":
                    ShowAllBookings();
                    break;
                case "3":
                    CreateBooking();
                    break;
                case "4":
                    CreateGiftCertificate();
                    break;
                case "5":
                    CancelBooking();
                    break;
                case "6":
                    AddNewClient();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
                Console.ReadKey();
            }
        }

        
        _dbContext.Dispose();
    }

    private static void InitializeServices()
    {
        _dbContext = new QuestRoomDbContext(new DbContextOptionsBuilder<QuestRoomDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuestRoomDb;Trusted_Connection=True;")
            .Options);

       
        var questRepository = new QuestRepository(_dbContext);
        var bookingRepository = new BookingRepository(_dbContext);
        var clientRepository = new ClientRepository(_dbContext);
        var certificateRepository = new GiftCertificateRepository(_dbContext);

        
        _questService = new QuestService(questRepository);
        _bookingService = new BookingService(bookingRepository, questRepository, certificateRepository);
        _clientService = new ClientService(clientRepository);
        _certificateService = new GiftCertificateService(certificateRepository);
    }

    private static void SeedData()
    {
        _dbContext.Database.EnsureCreated();

        if (!_dbContext.Quests.Any())
        {
            var quests = new[]
            {
                new Quest { Name = "Загадка стародавнього замку", Description = "Розкрийте таємницю, що приховує стародавній замок.", MaxParticipants = 6, DurationMinutes = 60, Price = 800M, DifficultyLevel = "Середній", ImageUrl = "placeholder.jpg" },
                new Quest { Name = "Втеча з в'язниці", Description = "Знайдіть шлях для втечі з добре охоронюваної в'язниці.", MaxParticipants = 4, DurationMinutes = 45, Price = 600M, DifficultyLevel = "Складний", ImageUrl = "placeholder.jpg" },
                new Quest { Name = "Піратський скарб", Description = "Знайдіть скарб, який заховав легендарний пірат.", MaxParticipants = 8, DurationMinutes = 90, Price = 1000M, DifficultyLevel = "Легкий", ImageUrl = "placeholder.jpg" }
            };
            _dbContext.Quests.AddRange(quests);

            var clients = new[]
            {
                new Client { Name = "Андрій Петренко", Email = "andrii@example.com", Phone = "0991234567" },
                new Client { Name = "Олена Ковальчук", Email = "olena@example.com", Phone = "0671234567" },
            };
            _dbContext.Clients.AddRange(clients);

            _dbContext.SaveChanges();
        }
    }

    private static void ShowAllQuests()
    {
        var quests = _questService.GetAllQuests();
        Console.WriteLine("=== Доступні квести ===");
        foreach (var quest in quests)
        {
            Console.WriteLine($"ID: {quest.Id}, Назва: {quest.Name}, Макс. учасників: {quest.MaxParticipants}, Тривалість: {quest.DurationMinutes} хв, Ціна: {quest.Price} грн");
        }
    }

    private static void ShowAllBookings()
    {
        var bookings = _bookingService.GetAllBookings();
        Console.WriteLine("=== Всі бронювання ===");
        foreach (var booking in bookings)
        {
            Console.WriteLine($"ID: {booking.Id}, Квест: {booking.Quest?.Name}, Клієнт: {booking.Client?.Name}, Дата: {booking.StartTime}, Статус: {booking.Status}");
        }
    }

    private static void CreateBooking()
    {
        Console.WriteLine("=== Створення нового бронювання ===");

        ShowAllQuests();
        Console.Write("Введіть ID квесту: ");
        if (!int.TryParse(Console.ReadLine(), out int questId))
        {
            Console.WriteLine("Помилка! Невірний формат ID.");
            return;
        }

        var quest = _questService.GetQuestById(questId);
        if (quest == null)
        {
            Console.WriteLine("Квест з таким ID не знайдено.");
            return;
        }

        var clients = _clientService.GetAllClients();
        Console.WriteLine("\n=== Доступні клієнти ===");
        foreach (Client client1 in clients)
        {
            Console.WriteLine($"ID: {client1.Id}, Ім'я: {client1.Name}, Email: {client1.Email}");
        }

        Console.Write("Введіть ID клієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int clientId))
        {
            Console.WriteLine("Помилка! Невірний формат ID.");
            return;
        }

        var client = _clientService.GetClientById(clientId);
        if (client == null)
        {
            Console.WriteLine("Клієнт з таким ID не знайдено.");
            return;
        }

        Console.Write("Введіть дату (формат: yyyy-MM-dd): ");
        var dateString = Console.ReadLine();

        Console.Write("Введіть час (формат: HH:mm): ");
        var timeString = Console.ReadLine();

        if (!DateTime.TryParse($"{dateString} {timeString}", out DateTime startTime))
        {
            Console.WriteLine("Помилка! Невірний формат дати або часу.");
            return;
        }

        Console.Write($"Введіть кількість учасників (макс. {quest.MaxParticipants}): ");
        if (!int.TryParse(Console.ReadLine(), out int participants) || participants <= 0 || participants > quest.MaxParticipants)
        {
            Console.WriteLine("Помилка! Невірна кількість учасників.");
            return;
        }

        Console.Write("Використати подарунковий сертифікат? (y/n): ");
        string useCertificate = Console.ReadLine()?.ToLower();

        string certificateCode = null;
        if (useCertificate == "y")
        {
            Console.Write("Введіть код сертифіката: ");
            certificateCode = Console.ReadLine();

            if (!_certificateService.IsValidCertificate(certificateCode))
            {
                Console.WriteLine("Помилка! Недійсний сертифікат або термін його дії закінчився.");
                return;
            }
        }

        bool success = _bookingService.CreateBooking(questId, clientId, startTime, participants, certificateCode);

        if (success)
            Console.WriteLine("Бронювання успішно створено!");
        else
            Console.WriteLine("Не вдалося створити бронювання. Перевірте чи часовий слот доступний.");
    }

    private static void AddNewClient()
    {
        Console.WriteLine("=== Додавання нового клієнта ===");

        Console.Write("Введіть ім'я клієнта: ");
        string name = Console.ReadLine();

        Console.Write("Введіть email клієнта: ");
        string email = Console.ReadLine();

        Console.Write("Введіть номер телефону клієнта: ");
        string phone = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
        {
            Console.WriteLine("Помилка! Всі поля повинні бути заповнені.");
            return;
        }

        var client = new Client
        {
            Name = name,
            Email = email,
            Phone = phone
        };

        _clientService.AddClient(client);

        Console.WriteLine($"Клієнт '{name}' успішно доданий з ID: {client.Id}");
    }

    private static void CreateGiftCertificate()
    {
        Console.WriteLine("=== Створення подарункового сертифіката ===");

        var clients = _clientService.GetAllClients();
        Console.WriteLine("\n=== Доступні клієнти ===");
        foreach (var client in clients)
        {
            Console.WriteLine($"ID: {client.Id}, Ім'я: {client.Name}, Email: {client.Email}");
        }

        Console.Write("Введіть ID клієнта (або залиште порожнім): ");
        string clientIdInput = Console.ReadLine();
        int? clientId = null;

        if (!string.IsNullOrEmpty(clientIdInput))
        {
            if (!int.TryParse(clientIdInput, out int id))
            {
                Console.WriteLine("Помилка! Невірний формат ID.");
                return;
            }
            clientId = id;
        }

        Console.Write("Сертифікат для конкретного квесту? (y/n): ");
        string forSpecificQuest = Console.ReadLine()?.ToLower();

        int? questId = null;
        if (forSpecificQuest == "y")
        {
            ShowAllQuests();
            Console.Write("Введіть ID квесту: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Помилка! Невірний формат ID.");
                return;
            }
            questId = id;
        }

        Console.Write("Термін дії (днів, за замовчуванням 180): ");
        string validityInput = Console.ReadLine();
        int validityDays = 180;

        if (!string.IsNullOrEmpty(validityInput) && !int.TryParse(validityInput, out validityDays))
        {
            Console.WriteLine("Помилка! Невірний формат числа днів.");
            return;
        }

        var certificate = _certificateService.CreateCertificate(clientId, questId, validityDays);

        Console.WriteLine($"Сертифікат успішно створено! Код: {certificate.Code}, Дійсний до: {certificate.ExpiryDate.ToShortDateString()}");
    }

    private static void CancelBooking()
    {
        Console.WriteLine("=== Скасування бронювання ===");

        var bookings = _bookingService.GetAllBookings();
        Console.WriteLine("=== Активні бронювання ===");
        foreach (var booking in bookings.Where(b => b.Status != "Скасовано"))
        {
            Console.WriteLine($"ID: {booking.Id}, Квест: {booking.Quest?.Name}, Клієнт: {booking.Client?.Name}, Дата: {booking.StartTime}");
        }

        Console.Write("Введіть ID бронювання для скасування: ");
        if (!int.TryParse(Console.ReadLine(), out int bookingId))
        {
            Console.WriteLine("Помилка! Невірний формат ID.");
            return;
        }

        bool success = _bookingService.CancelBooking(bookingId);

        if (success)
            Console.WriteLine("Бронювання успішно скасовано!");
        else
            Console.WriteLine("Помилка при скасуванні бронювання. Перевірте ID.");
    }
}