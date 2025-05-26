using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestRoom.DAL;
using QuestRoom.BLL.Services;
using QuestRoom.DAL.QuestRoom.DAL;
using QuestRoom.DAL.Repositories;
using QuestRoom.DAL.UnitOfWork;
using QuestRoom.DAL.Entities;

namespace appz4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створення сервісів
            var services = new ServiceCollection();

            // Реєстрація контексту БД
            services.AddDbContext<QuestRoomDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuestRoomDb;Trusted_Connection=True;"));

            // Реєстрація репозиторіїв
            services.AddScoped<IRepository<Quest>, Repository<Quest>>();
            services.AddScoped<IRepository<Booking>, Repository<Booking>>();
            services.AddScoped<IRepository<Client>, Repository<Client>>();
            services.AddScoped<IRepository<GiftCertificate>, Repository<GiftCertificate>>();

            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IGiftCertificateRepository, GiftCertificateRepository>();

            // Реєстрація Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Реєстрація сервісів
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IGiftCertificateService, GiftCertificateService>();

            // Реєстрація UI контролера
            services.AddScoped<QuestRoomUIController>();

            // Створення провайдера сервісів
            var serviceProvider = services.BuildServiceProvider();

            // Отримання UI контролера
            var uiController = serviceProvider.GetRequiredService<QuestRoomUIController>();

            // Заповнення тестовими даними, якщо потрібно
            uiController.SeedData();

            // Запуск головного меню
            RunMainMenu(uiController);
        }

        private static void RunMainMenu(QuestRoomUIController controller)
        {
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
                        controller.ShowAllQuests();
                        break;
                    case "2":
                        controller.ShowAllBookings();
                        break;
                    case "3":
                        controller.CreateBooking();
                        break;
                    case "4":
                        controller.CreateGiftCertificate();
                        break;
                    case "5":
                        controller.CancelBooking();
                        break;
                    case "6":
                        controller.AddNewClient();
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
        }
    }
}