using Microsoft.EntityFrameworkCore;
using QuestRoom.BLL.Services;
using QuestRoom.DAL;
using QuestRoom.DAL.Entities;
using QuestRoom.DAL.QuestRoom.DAL;
using QuestRoom.DAL.Repositories;
using QuestRoom.DAL.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Реєстрація контролерів
builder.Services.AddControllers();

// Налаштування Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Реєстрація контексту бази даних
builder.Services.AddDbContext<QuestRoomDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
    "Server=(localdb)\\mssqllocaldb;Database=QuestRoomDb;Trusted_Connection=True;"));

// Реєстрація загальних репозиторіїв
builder.Services.AddScoped<IRepository<Quest>, Repository<Quest>>();
builder.Services.AddScoped<IRepository<Booking>, Repository<Booking>>();
builder.Services.AddScoped<IRepository<Client>, Repository<Client>>();
builder.Services.AddScoped<IRepository<GiftCertificate>, Repository<GiftCertificate>>();

// Реєстрація спеціалізованих репозиторіїв
builder.Services.AddScoped<IQuestRepository, QuestRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IGiftCertificateRepository, GiftCertificateRepository>();

// Реєстрація Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Реєстрація сервісів бізнес-логіки
builder.Services.AddScoped<IQuestService, QuestService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IGiftCertificateService, GiftCertificateService>();

// Налаштування CORS (для фронтенд додатків)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Ініціалізація бази даних з тестовими даними
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<QuestRoomDbContext>();

        // Створення бази даних, якщо не існує
        context.Database.EnsureCreated();

        // Заповнення тестовими даними
        SeedData(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

// Метод для заповнення тестовими даними
static void SeedData(IServiceProvider services)
{
    var questService = services.GetRequiredService<IQuestService>();
    var clientService = services.GetRequiredService<IClientService>();

    // Перевіряємо чи є дані в базі
    if (!questService.GetAllQuests().Any())
    {
        // Додаємо квести
        var quests = new[]
        {
            new Quest
            {
                Name = "Загадка стародавнього замку",
                Description = "Розкрийте таємницю, що приховує стародавній замок.",
                MaxParticipants = 6,
                DurationMinutes = 60,
                Price = 800M,
                DifficultyLevel = "Середній",
                ImageUrl = "placeholder.jpg",
                IsActive = true
            },
            new Quest
            {
                Name = "Втеча з в'язниці",
                Description = "Знайдіть шлях для втечі з добре охоронюваної в'язниці.",
                MaxParticipants = 4,
                DurationMinutes = 45,
                Price = 600M,
                DifficultyLevel = "Складний",
                ImageUrl = "placeholder.jpg",
                IsActive = true
            },
            new Quest
            {
                Name = "Піратський скарб",
                Description = "Знайдіть скарб, який заховав легендарний пірат.",
                MaxParticipants = 8,
                DurationMinutes = 90,
                Price = 1000M,
                DifficultyLevel = "Легкий",
                ImageUrl = "placeholder.jpg",
                IsActive = true
            }
        };

        foreach (var quest in quests)
        {
            questService.AddQuest(quest);
        }

        // Додаємо клієнтів
        var clients = new[]
        {
            new Client { Name = "Андрій Петренко", Email = "andrii@example.com", Phone = "0991234567" },
            new Client { Name = "Олена Ковальчук", Email = "olena@example.com", Phone = "0671234567" },
            new Client { Name = "Микола Іваненко", Email = "mykola@example.com", Phone = "0501234567" }
        };

        foreach (var client in clients)
        {
            clientService.AddClient(client);
        }
    }
}