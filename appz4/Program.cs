using System;
using Microsoft.EntityFrameworkCore;
using QuestRoom.DAL;
using QuestRoom.BLL.Services;
using QuestRoom.DAL.QuestRoom.DAL;

namespace appz4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Ініціалізація контексту бази даних
            var dbContext = new QuestRoomDbContext(new DbContextOptionsBuilder<QuestRoomDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuestRoomDb;Trusted_Connection=True;")
                .Options);

            // Ініціалізація контролера UI
            var uiController = new QuestRoomUIController(dbContext);

            // Заповнення тестовими даними, якщо потрібно
            uiController.SeedData();

            // Запуск головного меню
            RunMainMenu(uiController);

            // Закриття контексту БД при виході
            dbContext.Dispose();
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