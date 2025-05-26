using Microsoft.AspNetCore.Mvc;
using QuestRoom.BLL.Services;
using QuestRoom.PL.Models;
using QuestRoom.DAL.Entities;

namespace QuestRoom.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IQuestService _questService;
        private readonly IClientService _clientService;

        public BookingController(
            IBookingService bookingService,
            IQuestService questService,
            IClientService clientService)
        {
            _bookingService = bookingService;
            _questService = questService;
            _clientService = clientService;
        }

        /// <summary>
        /// Отримати всі бронювання
        /// </summary>
        /// <returns>Список бронювань</returns>
        [HttpGet]
        public ActionResult<ApiResponse<List<BookingDto>>> GetAllBookings()
        {
            try
            {
                var bookings = _bookingService.GetAllBookings();
                var bookingDtos = bookings.Select(b => new BookingDto
                {
                    Id = b.Id,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    ParticipantsCount = b.ParticipantsCount,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    IsPaid = b.IsPaid,
                    QuestId = b.QuestId,
                    QuestName = b.Quest?.Name ?? "Невідомий квест",
                    ClientId = b.ClientId,
                    ClientName = b.Client?.Name ?? "Невідомий клієнт",
                    GiftCertificateId = b.GiftCertificateId,
                    GiftCertificateCode = b.UsedGiftCertificate?.Code
                }).ToList();

                return Ok(ApiResponse<List<BookingDto>>.SuccessResult(bookingDtos, "Бронювання успішно отримані"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<BookingDto>>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Отримати бронювання за ID
        /// </summary>
        /// <param name="id">ID бронювання</param>
        /// <returns>Бронювання</returns>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<BookingDto>> GetBookingById(int id)
        {
            try
            {
                var booking = _bookingService.GetBookingById(id);
                if (booking == null)
                {
                    return NotFound(ApiResponse<BookingDto>.ErrorResult("Бронювання не знайдено"));
                }

                var bookingDto = new BookingDto
                {
                    Id = booking.Id,
                    BookingDate = booking.BookingDate,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    ParticipantsCount = booking.ParticipantsCount,
                    TotalPrice = booking.TotalPrice,
                    Status = booking.Status,
                    IsPaid = booking.IsPaid,
                    QuestId = booking.QuestId,
                    QuestName = booking.Quest?.Name ?? "Невідомий квест",
                    ClientId = booking.ClientId,
                    ClientName = booking.Client?.Name ?? "Невідомий клієнт",
                    GiftCertificateId = booking.GiftCertificateId,
                    GiftCertificateCode = booking.UsedGiftCertificate?.Code
                };

                return Ok(ApiResponse<BookingDto>.SuccessResult(bookingDto, "Бронювання успішно отримано"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BookingDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Отримати бронювання для конкретного квесту
        /// </summary>
        /// <param name="questId">ID квесту</param>
        /// <returns>Список бронювань квесту</returns>
        [HttpGet("quest/{questId}")]
        public ActionResult<ApiResponse<List<BookingDto>>> GetBookingsByQuestId(int questId)
        {
            try
            {
                var bookings = _bookingService.GetBookingsByQuestId(questId);
                var bookingDtos = bookings.Select(b => new BookingDto
                {
                    Id = b.Id,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    ParticipantsCount = b.ParticipantsCount,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    IsPaid = b.IsPaid,
                    QuestId = b.QuestId,
                    QuestName = b.Quest?.Name ?? "Невідомий квест",
                    ClientId = b.ClientId,
                    ClientName = b.Client?.Name ?? "Невідомий клієнт",
                    GiftCertificateId = b.GiftCertificateId,
                    GiftCertificateCode = b.UsedGiftCertificate?.Code
                }).ToList();

                return Ok(ApiResponse<List<BookingDto>>.SuccessResult(bookingDtos, "Бронювання квесту успішно отримані"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<BookingDto>>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Створити нове бронювання
        /// </summary>
        /// <param name="createBookingDto">Дані для створення бронювання</param>
        /// <returns>Створене бронювання</returns>
        [HttpPost]
        public ActionResult<ApiResponse<BookingDto>> CreateBooking([FromBody] CreateBookingDto createBookingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<BookingDto>.ErrorResult("Невалідні дані"));
                }

                // Перевіряємо чи існують квест і клієнт
                var quest = _questService.GetQuestById(createBookingDto.QuestId);
                if (quest == null)
                {
                    return BadRequest(ApiResponse<BookingDto>.ErrorResult("Квест не знайдено"));
                }

                var client = _clientService.GetClientById(createBookingDto.ClientId);
                if (client == null)
                {
                    return BadRequest(ApiResponse<BookingDto>.ErrorResult("Клієнт не знайдено"));
                }

                // Створюємо бронювання
                var success = _bookingService.CreateBooking(
                    createBookingDto.QuestId,
                    createBookingDto.ClientId,
                    createBookingDto.StartTime,
                    createBookingDto.ParticipantsCount,
                    createBookingDto.GiftCertificateCode);

                if (!success)
                {
                    return BadRequest(ApiResponse<BookingDto>.ErrorResult("Не вдалося створити бронювання. Перевірте дані або доступність часового слоту."));
                }

                return Ok(ApiResponse<BookingDto>.SuccessResult(null, "Бронювання успішно створено"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BookingDto>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }

        /// <summary>
        /// Скасувати бронювання
        /// </summary>
        /// <param name="id">ID бронювання</param>
        /// <returns>Результат операції</returns>
        [HttpPut("{id}/cancel")]
        public ActionResult<ApiResponse<object>> CancelBooking(int id)
        {
            try
            {
                var booking = _bookingService.GetBookingById(id);
                if (booking == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResult("Бронювання не знайдено"));
                }

                var success = _bookingService.CancelBooking(id);
                if (!success)
                {
                    return BadRequest(ApiResponse<object>.ErrorResult("Не вдалося скасувати бронювання"));
                }

                return Ok(ApiResponse<object>.SuccessResult(null, "Бронювання успішно скасовано"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult($"Помилка сервера: {ex.Message}"));
            }
        }
    }
}