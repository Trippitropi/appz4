using Microsoft.EntityFrameworkCore;
using QuestRoom.BLL.Services;
using QuestRoom.DAL;
using QuestRoom.DAL.Entities;
using QuestRoom.DAL.QuestRoom.DAL;
using QuestRoom.DAL.Repositories;
using QuestRoom.DAL.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ��������� ����������
builder.Services.AddControllers();

// ������������ Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��������� ��������� ���� �����
builder.Services.AddDbContext<QuestRoomDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
    "Server=(localdb)\\mssqllocaldb;Database=QuestRoomDb;Trusted_Connection=True;"));

// ��������� ��������� ����������
builder.Services.AddScoped<IRepository<Quest>, Repository<Quest>>();
builder.Services.AddScoped<IRepository<Booking>, Repository<Booking>>();
builder.Services.AddScoped<IRepository<Client>, Repository<Client>>();
builder.Services.AddScoped<IRepository<GiftCertificate>, Repository<GiftCertificate>>();

// ��������� �������������� ����������
builder.Services.AddScoped<IQuestRepository, QuestRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IGiftCertificateRepository, GiftCertificateRepository>();

// ��������� Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ��������� ������ �����-�����
builder.Services.AddScoped<IQuestService, QuestService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IGiftCertificateService, GiftCertificateService>();

// ������������ CORS (��� �������� �������)
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

// ����������� ���� ����� � ��������� ������
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<QuestRoomDbContext>();

        // ��������� ���� �����, ���� �� ����
        context.Database.EnsureCreated();

        // ���������� ��������� ������
        SeedData(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

// ����� ��� ���������� ��������� ������
static void SeedData(IServiceProvider services)
{
    var questService = services.GetRequiredService<IQuestService>();
    var clientService = services.GetRequiredService<IClientService>();

    // ���������� �� � ��� � ���
    if (!questService.GetAllQuests().Any())
    {
        // ������ ������
        var quests = new[]
        {
            new Quest
            {
                Name = "������� ������������� �����",
                Description = "��������� �������, �� ������� ���������� �����.",
                MaxParticipants = 6,
                DurationMinutes = 60,
                Price = 800M,
                DifficultyLevel = "�������",
                ImageUrl = "placeholder.jpg",
                IsActive = true
            },
            new Quest
            {
                Name = "����� � �'������",
                Description = "������� ���� ��� ����� � ����� ����������� �'������.",
                MaxParticipants = 4,
                DurationMinutes = 45,
                Price = 600M,
                DifficultyLevel = "��������",
                ImageUrl = "placeholder.jpg",
                IsActive = true
            },
            new Quest
            {
                Name = "ϳ�������� �����",
                Description = "������� �����, ���� ������� ����������� ����.",
                MaxParticipants = 8,
                DurationMinutes = 90,
                Price = 1000M,
                DifficultyLevel = "������",
                ImageUrl = "placeholder.jpg",
                IsActive = true
            }
        };

        foreach (var quest in quests)
        {
            questService.AddQuest(quest);
        }

        // ������ �볺���
        var clients = new[]
        {
            new Client { Name = "����� ��������", Email = "andrii@example.com", Phone = "0991234567" },
            new Client { Name = "����� ���������", Email = "olena@example.com", Phone = "0671234567" },
            new Client { Name = "������ ��������", Email = "mykola@example.com", Phone = "0501234567" }
        };

        foreach (var client in clients)
        {
            clientService.AddClient(client);
        }
    }
}